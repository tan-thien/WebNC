using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;

namespace WebShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
