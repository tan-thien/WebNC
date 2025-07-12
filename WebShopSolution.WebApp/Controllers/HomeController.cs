using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShopSolution.WebApp.Models;
using WebShopSolution.Application.Catalog.Products;
using Microsoft.Extensions.Configuration;
namespace WebShopSolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IProductService productService, IConfiguration configuration)
        {
            _logger = logger;
            _productService = productService;
            _configuration = configuration;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();

            // Lọc chỉ lấy sản phẩm có Status = "Active"
            var activeProducts = products.Where(p => p.Status == "Active").ToList();

            var adminBaseUrl = _configuration["AdminBaseUrl"];

            foreach (var product in activeProducts)
            {
                foreach (var image in product.ProductImages)
                {
                    if (!string.IsNullOrEmpty(image.ImagePath))
                    {
                        image.ImagePath = $"{adminBaseUrl}{image.ImagePath}";
                    }
                }
            }

            // Tạo đường dẫn no-image đầy đủ
            ViewBag.NoImagePath = $"{adminBaseUrl}/images/no-image.png";

            return View(activeProducts);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Store()
        {
            return View();
        }
        public IActionResult Gioithieu()
        {
            return View();
        }
    }
}
