using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // để dùng Session
using System.Threading.Tasks;
using WebShopSolution.Application.Catalog.Accounts;
using WebShopSolution.ViewModels.Catalog.Account;
using WebShopSolution.Application.Catalog.Customers;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

        public AccountController(IAccountService accountService, ICustomerService customerService)
        {
            _accountService = accountService;
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var account = await _accountService.LoginAsync(request);
            if (account == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(request);
            }

            // ✅ Kiểm tra Role
            if (account.Role != "User")
            {
                ModelState.AddModelError("", "Chỉ tài khoản người dùng (User) mới được phép đăng nhập.");
                return View(request);
            }

            // Lưu thông tin đăng nhập vào Session
            HttpContext.Session.SetInt32("UserId", account.IdAcc);
            HttpContext.Session.SetString("UserName", account.UserName);
            HttpContext.Session.SetString("Role", account.Role);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Xoá session khi đăng xuất
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var idAcc = await _accountService.RegisterAsync(request);
            if (idAcc == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                return View(request);
            }

            // Tạo thông tin Customer
            var customer = new Customer
            {
                CusName = request.CusName,
                Phone = request.Phone,
                Address = request.Address,
                IdAcc = idAcc.Value
            };
            await _customerService.AddAsync(customer);

            TempData["SuccessMessage"] = "Đăng ký thành công! Mời bạn đăng nhập.";
            return RedirectToAction("Login");
        }
    }
}
