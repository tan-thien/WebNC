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
            var entities = await _categoryRepository.GetAllAsync();

            // Map sang viewmodel
            var lookup = entities.Select(c => new CategoryViewModel
            {
                IdCate = c.IdCate,
                CateName = c.CateName,
                ParentId = c.ParentId
            }).ToList();

            // Build cây
            var dict = lookup.ToDictionary(c => c.IdCate);
            foreach (var item in lookup)
            {
                if (item.ParentId.HasValue && dict.ContainsKey(item.ParentId.Value))
                {
                    dict[item.ParentId.Value].Children.Add(item);
                }
            }

            return lookup.Where(c => !c.ParentId.HasValue).ToList(); // Trả về root nodes
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
                CateName = request.CateName,
                ParentId = request.ParentId
            };

            await _categoryRepository.AddAsync(newCategory);

            return new CategoryViewModel
            {
                IdCate = newCategory.IdCate,
                CateName = newCategory.CateName,
                ParentId = newCategory.ParentId, 
                Children = new List<CategoryViewModel>() 
            };
        }

        public async Task<bool> UpdateAsync(CategoryUpdateRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.IdCate);
            if (category == null) return false;

            category.CateName = request.CateName;
            category.ParentId = request.ParentId;

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

        public async Task<List<CategoryViewModel>> GetNestedCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            // Bước 1: Map danh sách sang CategoryViewModel
            var viewModels = categories.Select(c => new CategoryViewModel
            {
                IdCate = c.IdCate,
                CateName = c.CateName,
                ParentId = c.ParentId
            }).ToList();

            // Bước 2: Tạo Dictionary để dễ tra cứu theo Id
            var lookup = viewModels.ToDictionary(c => c.IdCate);

            // Bước 3: Gắn các danh mục con vào cha
            foreach (var item in viewModels)
            {
                if (item.ParentId.HasValue && lookup.ContainsKey(item.ParentId.Value))
                {
                    var parent = lookup[item.ParentId.Value];
                    parent.Children.Add(item);
                }
            }

            // Bước 4: Trả về danh sách root (danh mục không có cha)
            return viewModels.Where(c => !c.ParentId.HasValue).ToList();
        }

    }
}
