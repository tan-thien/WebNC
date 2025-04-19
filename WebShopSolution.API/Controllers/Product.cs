using Microsoft.AspNetCore.Mvc;
using WebShopSolution.Application.Catalog.Products;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _productService.CreateAsync(request);
            if (productId == 0)
                return BadRequest("Tạo sản phẩm thất bại.");

            return CreatedAtAction(nameof(GetById), new { id = productId }, request);
        }

        // PUT: api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != request.IdProduct)
                return BadRequest("Id không khớp.");

            var result = await _productService.UpdateAsync(request);
            if (!result)
                return NotFound();

            return Ok("Cập nhật thành công.");
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return Ok("Xóa thành công.");
        }
    }
}
