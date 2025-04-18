using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.Repositories.Categories;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Category;

namespace WebShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryViewModel>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryViewModel
            {
                IdCate = c.IdCate,
                CateName = c.CateName
            }).ToList();
        }

        public async Task<CategoryViewModel?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryViewModel
            {
                IdCate = category.IdCate,
                CateName = category.CateName
            };
        }

        public async Task<CategoryViewModel> AddAsync(CategoryCreateRequest request)
        {
            var newCategory = new Category
            {
                CateName = request.CateName
            };

            await _categoryRepository.AddAsync(newCategory);

            return new CategoryViewModel
            {
                IdCate = newCategory.IdCate,
                CateName = newCategory.CateName
            };
        }

        public async Task<bool> UpdateAsync(CategoryUpdateRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.IdCate);
            if (category == null) return false;

            category.CateName = request.CateName;
            await _categoryRepository.UpdateAsync(category);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(id);
            return true;
        }
    }
}
