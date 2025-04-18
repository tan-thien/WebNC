using Microsoft.AspNetCore.Mvc;
using WebShopSolution.Application.Catalog.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Category;

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

        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> CreateCategory([FromBody] CategoryCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            var newCategory = await _categoryService.AddAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.IdCate }, newCategory);
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequest request)
        {
            if (id != request.IdCate)
            {
                return BadRequest("Category ID mismatch.");
            }

            var updated = await _categoryService.UpdateAsync(request);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
