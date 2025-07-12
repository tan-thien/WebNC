using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.Application.Catalog.Accounts;
using WebShopSolution.Data.EF;
using WebShopSolution.ViewModels.Catalog.Account;

namespace WebShopSolution.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAsync();
            return View(accounts);
        }

        public IActionResult Create()
        {
            return View(new AccountCreateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            await _accountService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            await _accountService.ChangeRoleAsync(id, role);
            return RedirectToAction(nameof(Index));
        }
    }

}
