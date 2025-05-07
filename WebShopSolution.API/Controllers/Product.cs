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

        // Lấy tất cả sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products); // Trả về danh sách sản phẩm
        }

        // Lấy chi tiết sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sản phẩm
            }
            return Ok(product); // Trả về chi tiết sản phẩm
        }

        // Thêm mới sản phẩm
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
        {
            // Kiểm tra thông tin nhập vào có hợp lệ không
            if (ModelState.IsValid)
            {
                // Tạo sản phẩm mới từ request
                await _productService.AddAsync(request);
                return Ok(new { message = "Product created successfully!" });
            }

            return BadRequest(ModelState); // Nếu dữ liệu không hợp lệ, trả về lỗi
        }

        // Cập nhật sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest request)
        {
            if (request == null || id != request.IdProduct)
            {
                return BadRequest("Invalid data."); // Trả về lỗi 400 nếu dữ liệu không hợp lệ
            }

            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu sản phẩm không tồn tại
            }

            await _productService.UpdateAsync(request);
            return NoContent(); // Trả về mã 204 nếu cập nhật thành công
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu sản phẩm không tồn tại
            }

            await _productService.DeleteAsync(id);
            return NoContent(); // Trả về mã 204 nếu xóa thành công
        }
    }
}
