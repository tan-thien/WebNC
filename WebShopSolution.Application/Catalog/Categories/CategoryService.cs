using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;

namespace WebShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
