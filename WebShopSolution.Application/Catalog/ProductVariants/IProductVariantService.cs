using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Product;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Application.Catalog.ProductVariants
{
    public interface IProductVariantService
    {
        Task<ProductVariantViewModel> CreateAsync(ProductVariantCreateRequest request);
        Task<List<ProductVariantViewModel>> GetByProductIdAsync(int productId);
    }
}
