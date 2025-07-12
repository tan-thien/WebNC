using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Order;

namespace WebShopSolution.Admin.Controllers
{
    public class AdminOrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminOrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var orders = await client.GetFromJsonAsync<List<OrderViewModel>>("https://localhost:7236/api/order/all");

            return View(orders);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var order = await client.GetFromJsonAsync<OrderViewModel>($"https://localhost:7236/api/order/{id}");
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(OrderStatusUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsJsonAsync("https://localhost:7236/api/order/updatestatus", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật trạng thái thành công";
            }
            else
            {
                TempData["Error"] = "Cập nhật trạng thái thất bại";
            }

            return RedirectToAction("Detail", new { id = request.OrderId });
        }
    }
}
