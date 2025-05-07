using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> GetAllWithCategoryAsync(); // Thêm phương thức mới
        Task<Product> GetByIdWithCategoryAsync(int id); // Phương thức lấy sản phẩm theo id với Category

        Task<Product?> GetByIdWithDetailsAsync(int id); // Chi tiết đầy đủ
    }
}
