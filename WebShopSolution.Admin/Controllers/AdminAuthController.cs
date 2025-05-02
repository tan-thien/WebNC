using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.Data.EF;
using WebShopSolution.ViewModels.Catalog.Account;

namespace WebShopSolution.Admin.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminAuthController(WebShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _context.Accounts
                .FirstOrDefaultAsync(x => x.UserName == model.UserName
                                       && x.PassWord == model.PassWord
                                       && x.Status == "Active"
                                       && x.Role == "Admin");

            if (admin == null)
            {
                ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }

            // Lưu session hoặc cookie nếu muốn
            HttpContext.Session.SetString("AdminUserName", admin.UserName);

            // Redirect vào trang admin dashboard
            return RedirectToAction("Index", "AdminDashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUserName");
            return RedirectToAction("Login");
        }
    }
}
