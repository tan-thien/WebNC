using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebShopSolution.Application.Catalog.Carts;
using WebShopSolution.ViewModels.Catalog.CartItem;
using WebShopSolution.Application.Catalog.Vouchers;

namespace WebShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IVoucherService _voucherService;
        private readonly IConfiguration _configuration;

        public CartController(
        ICartService cartService,
        IVoucherService voucherService, // thêm
        IConfiguration configuration)
        {
            _cartService = cartService;
            _voucherService = voucherService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;
            if (cartId == 0)
            {
                cartId = await _cartService.GetOrCreateCartIdByUserId(userId.Value);
                HttpContext.Session.SetInt32("CartId", cartId);
            }

            var cartItems = _cartService.GetCartItems(cartId);
            var baseUrl = _configuration["AdminBaseUrl"];
            foreach (var item in cartItems)
                item.ImagePath = string.IsNullOrEmpty(item.ImagePath) || item.ImagePath.StartsWith("http")
                    ? $"{baseUrl}/images/no-image.png"
                    : $"{baseUrl}/{item.ImagePath.TrimStart('/')}";

            var vouchers = await _voucherService.GetAvailableVouchersAsync();
            ViewBag.Vouchers = vouchers;

            return View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCartAjax([FromBody] CartItemCreateRequest request)
        {
            if (request == null || request.IdProduct <= 0 || request.Quantity <= 0)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ." });

            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;
            if (cartId == 0)
            {
                cartId = await _cartService.GetOrCreateCartIdByUserId(userId.Value);
                HttpContext.Session.SetInt32("CartId", cartId);
            }

            request.IdCart = cartId;

            var result = await _cartService.AddToCartAsync(request);

            if (!result)
                return BadRequest(new { success = false, message = "Không thể thêm sản phẩm vào giỏ hàng." });

            var cartItems = _cartService.GetCartItems(cartId);
            var totalQuantity = cartItems.Sum(i => i.Quantity);

            return Ok(new { success = true, cartItemCount = totalQuantity });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartUpdateRequest request)
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;
            if (cartId == 0)
                return Json(new { success = false, message = "Không tìm thấy giỏ hàng." });

            var success = await _cartService.UpdateQuantityAsync(cartId, request.ProductId, request.VariantId, request.Quantity);

            if (!success)
                return Json(new { success = false, message = "Cập nhật số lượng thất bại." });

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem([FromBody] CartDeleteRequest request)
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;
            if (cartId == 0)
                return Json(new { success = false, message = "Không tìm thấy giỏ hàng." });

            var success = await _cartService.DeleteItemAsync(cartId, request.ProductId, request.VariantId);

            if (!success)
                return Json(new { success = false, message = "Xóa sản phẩm thất bại." });

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> SaveSelectedItemsToSession([FromBody] SaveSelectedCartRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return Json(new { success = false, message = "Không có sản phẩm nào được chọn." });

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập." });

            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;
            if (cartId == 0)
            {
                cartId = await _cartService.GetOrCreateCartIdByUserId(userId.Value);
                HttpContext.Session.SetInt32("CartId", cartId);
            }

            var adminBaseUrl = _configuration["AdminBaseUrl"];
            var cartItems = _cartService.GetCartItems(cartId);
            var sanitizedItems = new List<CartItemSelectionRequest>();

            foreach (var item in request.Items)
            {
                int productId = item.IdProduct;
                string productName = "";
                string imagePath = "";
                string variantInfo = "";

                if (item.VariantId.HasValue && item.IdProduct == 0)
                {
                    var variant = await _cartService.GetProductVariantByIdAsync(item.VariantId.Value);
                    if (variant != null)
                    {
                        productId = variant.ProductId;
                        item.IdProduct = productId;
                        variantInfo = variant.ProductVariantAttributes != null
                            ? string.Join(", ", variant.ProductVariantAttributes.Select(a => $"{a.AttributeName}: {a.AttributeValue}"))
                            : "";
                    }
                }

                var cartItem = cartItems.FirstOrDefault(x =>
                    x.ProductId == productId && x.VariantId == item.VariantId);

                if (cartItem != null)
                {
                    productName = cartItem.ProductName;
                    imagePath = !string.IsNullOrEmpty(cartItem.ImagePath) && cartItem.ImagePath.StartsWith("http")
                        ? new Uri(cartItem.ImagePath).AbsolutePath
                        : cartItem.ImagePath;
                }

                sanitizedItems.Add(new CartItemSelectionRequest
                {
                    ProductId = productId,
                    VariantId = item.VariantId,
                    Quantity = item.Quantity,
                    Price = Convert.ToInt32(item.Price),
                    ProductName = productName,
                    ImagePath = imagePath,
                    VariantInfo = variantInfo
                });
            }

            HttpContext.Session.SetString("SelectedCartItems", JsonConvert.SerializeObject(sanitizedItems));
            HttpContext.Session.SetString("SelectedCartTotal", request.TotalAmount.ToString());

            // ❗ Xử lý voucher (nếu có)
            if (!string.IsNullOrWhiteSpace(request.VoucherCode))
            {
                HttpContext.Session.SetString("SelectedVoucherCode", request.VoucherCode);

                // 👉 Gọi service tính giảm giá
                var result = await _voucherService.PreviewVoucherDiscountAsync(request.VoucherCode.Trim(), request.TotalAmount);

                if (result.IsValid)
                {
                    HttpContext.Session.SetInt32("SelectedDiscountAmount", result.DiscountAmount);
                    HttpContext.Session.SetString("IsVoucherFromCart", "true");
                }
                else
                {
                    // Nếu không hợp lệ thì xóa các giá trị cũ
                    HttpContext.Session.Remove("SelectedVoucherCode");
                    HttpContext.Session.Remove("SelectedDiscountAmount");
                    HttpContext.Session.Remove("IsVoucherFromCart");
                }
            }
            else
            {
                HttpContext.Session.Remove("SelectedVoucherCode");
                HttpContext.Session.Remove("SelectedDiscountAmount");
                HttpContext.Session.Remove("IsVoucherFromCart");
            }

            return Json(new { success = true });
        }
    }
}
