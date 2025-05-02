using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Category;


namespace WebShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllAsync();
        Task<CategoryViewModel?> GetByIdAsync(int id);
        Task<CategoryViewModel> AddAsync(CategoryCreateRequest request);
        Task<bool> UpdateAsync(CategoryUpdateRequest request);
        Task<bool> DeleteAsync(int id);

        Task<List<CategoryViewModel>> GetNestedCategoriesAsync();

    }

}
