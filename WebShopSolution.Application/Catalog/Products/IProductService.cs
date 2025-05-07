using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Product;


namespace WebShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetAllAsync();  // Đổi tên phương thức cho đồng bộ
        Task<ProductViewModel> GetByIdAsync(int id);  // Đổi tên phương thức cho đồng bộ
        Task AddAsync(ProductCreateRequest request);  // Đổi phương thức để sử dụng ProductCreateRequest
        Task UpdateAsync(ProductUpdateRequest request);  // Đổi phương thức để sử dụng ProductUpdateRequest
        Task DeleteAsync(int id);
    }

}
