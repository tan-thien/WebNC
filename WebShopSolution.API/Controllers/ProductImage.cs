using Microsoft.AspNetCore.Mvc;
using WebShopSolution.ViewModels.Catalog.Product;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.EF; // namespace chứa ApplicationDbContext
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShopSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly WebShopDbContext _context;

        public ProductImageController(WebShopDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productImage = new ProductImage
            {
                ProductId = request.ProductId,
                ImagePath = request.ImagePath,
                SortOrder = request.SortOrder
            };

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return Ok(productImage); // trả lại object vừa tạo
        }

        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var images = await _context.ProductImages
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductImageViewModel
                {
                    Id = p.Id,
                    ImagePath = p.ImagePath,
                    SortOrder = p.SortOrder,
                    ProductId = p.ProductId
                })
                .ToListAsync();

            return Ok(images);
        }

    }
}
