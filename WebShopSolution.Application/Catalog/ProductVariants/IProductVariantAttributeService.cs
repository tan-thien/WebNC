using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.Application.Catalog.ProductVariants
{
    public interface IProductVariantAttributeService
    {
        Task<int> CreateAsync(ProductVariantAttributeCreateRequest request);
    }
}
