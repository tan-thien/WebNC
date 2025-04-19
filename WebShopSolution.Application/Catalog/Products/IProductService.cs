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
        Task<List<ProductViewModel>> GetAllAsync();
        Task<ProductViewModel?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProductCreateRequest request);
        Task<bool> UpdateAsync(ProductUpdateRequest request);
        Task<bool> DeleteAsync(int id);
    }

}
