using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebShopSolution.Application.Catalog.Categories;
using WebShopSolution.Application.Catalog.Products;
using WebShopSolution.Application.Catalog.Vouchers;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IVoucherService _voucherService; 
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;

        public ProductController(
        IProductService productService,
        IVoucherService voucherService, 
        IConfiguration configuration,
         ICategoryService categoryService)
        {
            _productService = productService;
            _voucherService = voucherService;
            _configuration = configuration;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            var adminBaseUrl = _configuration["AdminBaseUrl"];
            foreach (var img in product.ProductImages)
                img.ImagePath = string.IsNullOrEmpty(img.ImagePath)
                    ? $"{adminBaseUrl}/images/no-image.png"
                    : $"{adminBaseUrl}{img.ImagePath}";

            var vouchers = await _voucherService.GetAvailableVouchersAsync();
            ViewBag.Vouchers = vouchers;

            return View(product);
        }

        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            const int pageSize = 15;

            var products = await _productService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            var activeProducts = products.Where(p => p.Status == "Active").ToList();

            if (categoryId.HasValue)
            {
                activeProducts = activeProducts.Where(p => p.IdCate == categoryId.Value).ToList();
            }

            var totalProducts = activeProducts.Count;
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var pagedProducts = activeProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var adminBaseUrl = _configuration["AdminBaseUrl"];
            foreach (var product in pagedProducts)
            {
                foreach (var image in product.ProductImages)
                {
                    if (!string.IsNullOrEmpty(image.ImagePath))
                    {
                        image.ImagePath = $"{adminBaseUrl}{image.ImagePath}";
                    }
                }
            }

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.NoImagePath = $"{adminBaseUrl}/images/no-image.png";

            var viewModel = new ProductListViewModel
            {
                Products = pagedProducts,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }



    }
}