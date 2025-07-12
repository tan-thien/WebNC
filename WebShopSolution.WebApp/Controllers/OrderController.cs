using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebShopSolution.Application.Catalog.Orders;
using WebShopSolution.Data.EF;
using WebShopSolution.ViewModels.Catalog.CartItem;
using WebShopSolution.ViewModels.Catalog.Order;
using WebShopSolution.ViewModels.Catalog.Voucher;

namespace WebShopSolution.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly WebShopDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, WebShopDbContext context, IConfiguration configuration)
        {
            _orderService = orderService;
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var selectedItemsJson = HttpContext.Session.GetString("SelectedCartItems");
            var totalAmountStr = HttpContext.Session.GetString("SelectedCartTotal");
            var voucherCode = HttpContext.Session.GetString("SelectedVoucherCode");
            var discountAmount = HttpContext.Session.GetInt32("SelectedDiscountAmount") ?? 0;
            var isVoucherFromCart = HttpContext.Session.GetString("IsVoucherFromCart") == "true";

            if (string.IsNullOrEmpty(selectedItemsJson) || string.IsNullOrEmpty(totalAmountStr))
            {
                TempData["ErrorMessage"] = "Không có sản phẩm nào để thanh toán.";
                return RedirectToAction("Index", "Cart");
            }

            var items = JsonConvert.DeserializeObject<List<CartItemSelectionRequest>>(selectedItemsJson);
            int totalAmount = int.TryParse(totalAmountStr, out var parsedAmount) ? parsedAmount : 0;

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.IdAcc == userId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Index", "Cart");
            }

            string adminBaseUrl = _configuration["AdminBaseUrl"];

            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                item.ProductName = product?.ProductName;

                if (item.VariantId.HasValue)
                {
                    var variant = await _context.ProductVariants
                        .Include(v => v.ProductVariantAttributes)
                        .FirstOrDefaultAsync(v => v.Id == item.VariantId.Value);

                    item.VariantInfo = variant != null
                        ? string.Join(", ", variant.ProductVariantAttributes.Select(a => $"{a.AttributeName}: {a.AttributeValue}"))
                        : "-";
                }
                else
                {
                    item.VariantInfo = "-";
                }

                var productImage = await _context.ProductImages
                    .Where(pi => pi.ProductId == item.ProductId)
                    .OrderBy(pi => pi.SortOrder)
                    .FirstOrDefaultAsync();

                item.ImagePath = productImage != null
                    ? $"{adminBaseUrl}/{productImage.ImagePath.TrimStart('/')}"
                    : $"{adminBaseUrl}/images/no-image.png";
            }

            var model = new OrderCreateRequest
            {
                Items = items,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                Phone = customer.Phone,
                ShippingAddress = customer.Address,
                VoucherCode = string.IsNullOrWhiteSpace(voucherCode) ? null : voucherCode,
                IsVoucherFromCart = isVoucherFromCart
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(OrderCreateRequest request)
        {
            if (request.Items == null || !request.Items.Any())
            {
                TempData["ErrorMessage"] = "Không có sản phẩm nào để đặt hàng.";
                return RedirectToAction("Index", "Cart");
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!string.IsNullOrEmpty(request.VoucherCode))
            {
                HttpContext.Session.SetString("SelectedVoucherCode", request.VoucherCode);
            }

            var result = await _orderService.CreateOrderAsync(userId.Value, request);

            if (!result)
            {
                ModelState.AddModelError("", "Đặt hàng thất bại. Vui lòng thử lại.");
                return View(request);
            }

            TempData["OrderSuccess"] = true;

            var adminBaseUrl = _configuration["AdminBaseUrl"];
            foreach (var item in request.Items)
            {
                if (string.IsNullOrEmpty(item.ImagePath))
                {
                    var productImage = await _context.ProductImages
                        .Where(pi => pi.ProductId == item.ProductId)
                        .OrderBy(pi => pi.SortOrder)
                        .FirstOrDefaultAsync();

                    item.ImagePath = productImage != null
                        ? $"{adminBaseUrl}/{productImage.ImagePath.TrimStart('/')}"
                        : $"{adminBaseUrl}/images/no-image.png";
                }
                else if (!item.ImagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    item.ImagePath = $"{adminBaseUrl}/{item.ImagePath.TrimStart('/')}";
                }
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var orders = await _orderService.GetOrdersByUserIdAsync(userId.Value);
            return View("History", orders);
        }
        [HttpPost]
        public async Task<IActionResult> ApplyVoucher([FromBody] ApplyVoucherRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.VoucherCode))
                return Json(new { success = false, message = "Vui lòng nhập mã." });

            var selectedItemsJson = HttpContext.Session.GetString("SelectedCartItems");
            var totalAmountStr = HttpContext.Session.GetString("SelectedCartTotal");

            if (string.IsNullOrEmpty(selectedItemsJson) || string.IsNullOrEmpty(totalAmountStr))
                return Json(new { success = false, message = "Không tìm thấy giỏ hàng." });

            var items = JsonConvert.DeserializeObject<List<CartItemSelectionRequest>>(selectedItemsJson);
            if (items == null || !items.Any())
                return Json(new { success = false, message = "Giỏ hàng trống." });

            if (!int.TryParse(totalAmountStr, out int totalAmount))
                return Json(new { success = false, message = "Tổng tiền không hợp lệ." });

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });

            var result = await _orderService.ApplyVoucherAsync(request.VoucherCode.Trim(), totalAmount, userId.Value);

            if (!result.IsValid)
                return Json(new { success = false, message = result.Message });

            // Lưu mã voucher vào session nếu hợp lệ
            HttpContext.Session.SetString("SelectedVoucherCode", request.VoucherCode);

            return Json(new
            {
                success = true,
                discountAmount = result.DiscountAmount,
                originalTotal = result.OriginalTotal,
                message = result.Message
            });
        }


    }
}
