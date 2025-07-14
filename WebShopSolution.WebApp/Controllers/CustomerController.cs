using Microsoft.AspNetCore.Mvc;
using WebShopSolution.Application.Catalog.Customers;
using WebShopSolution.ViewModels.Catalog.Customer;
namespace WebShopSolution.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var customer = await _customerService.GetByAccountIdAsync(userId.Value);
            if (customer == null || customer.Account == null) return NotFound();

            var model = new CustomerProfileViewModel
            {
                CusName = customer.CusName,
                Phone = customer.Phone,
                Address = customer.Address,
                UserName = customer.Account.UserName,
                PassWord = customer.Account.PassWord // ✅ hiển thị mật khẩu mã hóa nếu cần
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var customer = await _customerService.GetByAccountIdAsync(userId.Value);
            if (customer == null || customer.Account == null) return NotFound();

            var model = new CustomerProfileViewModel
            {
                CusName = customer.CusName,
                Phone = customer.Phone,
                Address = customer.Address,
                UserName = customer.Account.UserName,
                PassWord = "",              // ✅ mật khẩu mới mặc định trống
                CurrentPassword = ""        // ✅ mật khẩu cũ trống
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(CustomerProfileViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var customer = await _customerService.GetByAccountIdAsync(userId.Value);
            if (customer == null || customer.Account == null)
                return NotFound();

            // Đảm bảo gán lại UserName từ dữ liệu gốc
            model.UserName = customer.Account.UserName;

            if (!ModelState.IsValid)
                return View(model);

            // ✅ Nếu có mật khẩu mới thì yêu cầu nhập đúng mật khẩu hiện tại
            if (!string.IsNullOrWhiteSpace(model.PassWord))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Vui lòng nhập mật khẩu hiện tại.");
                    return View(model);
                }

                if (model.CurrentPassword != customer.Account.PassWord)
                {
                    ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không chính xác.");
                    return View(model);
                }

                customer.Account.PassWord = model.PassWord;
            }

            // Cập nhật thông tin khác
            customer.CusName = model.CusName;
            customer.Phone = model.Phone;
            customer.Address = model.Address;

            await _customerService.UpdateAsync(customer);

            TempData["Success"] = "Cập nhật thành công.";
            return RedirectToAction("Profile");
        }



    }
}
