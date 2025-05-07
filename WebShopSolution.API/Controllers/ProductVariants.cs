using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebShopSolution.Application.Catalog.ProductVariant;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantsController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        // POST: api/ProductVariants
        [HttpPost]


        // GET: api/ProductVariants/product/5
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var variants = await _productVariantService.GetByProductIdAsync(productId);
            return Ok(variants);
        }
    }
}
