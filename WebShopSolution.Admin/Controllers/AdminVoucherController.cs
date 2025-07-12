using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Voucher;

namespace WebShopSolution.Admin.Controllers
{
    public class AdminVoucherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminVoucherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var vouchers = await client.GetFromJsonAsync<List<Voucher>>("https://localhost:7236/api/voucher/all");
            return View(vouchers);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var voucher = await client.GetFromJsonAsync<Voucher>($"https://localhost:7236/api/voucher/{id}");
            if (voucher == null) return NotFound();

            // ✨ Ánh xạ từ Entity -> ViewModel
            var updateModel = new VoucherUpdateRequest
            {
                VoucherId = voucher.VoucherId,
                Code = voucher.Code,
                Name = voucher.Name,
                Description = voucher.Description,
                DiscountType = voucher.DiscountType,
                DiscountValue = voucher.DiscountValue,
                MaxDiscountValue = voucher.MaxDiscountValue,
                Quantity = voucher.Quantity,
                UserLimit = voucher.UserLimit,
                StartDate = voucher.StartDate,
                EndDate = voucher.EndDate,
                IsActive = voucher.IsActive
            };

            return View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VoucherUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request); // Trả lại view nếu có lỗi nhập

            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsJsonAsync("https://localhost:7236/api/voucher/update", request);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Cập nhật thất bại.");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7236/api/voucher/create", request);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Tạo voucher thất bại.");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"https://localhost:7236/api/voucher/delete/{id}");
            return RedirectToAction("Index");
        }
    }
}
