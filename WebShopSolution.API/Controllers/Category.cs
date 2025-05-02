using Microsoft.AspNetCore.Mvc;
using WebShopSolution.Application.Catalog.Categories;
using WebShopSolution.ViewModels.Catalog.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebShopSolution.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Lấy danh sách tất cả danh mục (có thể gồm danh mục lồng nhau nếu service xử lý)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Lấy danh mục theo ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryViewModel>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        /// <summary>
        /// Tạo mới danh mục
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> Create([FromBody] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCategory = await _categoryService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.IdCate }, createdCategory);
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequest request)
        {
            if (id != request.IdCate)
                return BadRequest("Category ID mismatch.");

            var result = await _categoryService.UpdateAsync(request);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Xoá danh mục
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("tree")]
        public async Task<ActionResult<List<CategoryViewModel>>> GetNestedCategories()
        {
            var tree = await _categoryService.GetNestedCategoriesAsync();
            return Ok(tree);
        }

    }
}
