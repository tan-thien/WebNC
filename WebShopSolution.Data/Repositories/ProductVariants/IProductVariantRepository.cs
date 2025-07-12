using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.ProductVariants
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id);
        Task<IEnumerable<ProductVariant>> GetWithIncludeAsync(Expression<Func<ProductVariant, bool>> predicate, string includeProperties = "");
    }
}
