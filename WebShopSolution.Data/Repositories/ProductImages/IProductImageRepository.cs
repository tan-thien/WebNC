using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.ProductImages
{
    public interface IProductImageRepository
    {
        Task<ProductImage?> GetFirstOrDefaultAsync(
            Expression<Func<ProductImage, bool>> predicate,
            Func<IQueryable<ProductImage>, IOrderedQueryable<ProductImage>>? orderBy = null);
    }
}
