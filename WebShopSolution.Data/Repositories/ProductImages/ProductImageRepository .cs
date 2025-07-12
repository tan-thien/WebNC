using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.ProductImages
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly WebShopDbContext _context;

        public ProductImageRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<ProductImage?> GetFirstOrDefaultAsync(
            Expression<Func<ProductImage, bool>> predicate,
            Func<IQueryable<ProductImage>, IOrderedQueryable<ProductImage>>? orderBy = null)
        {
            IQueryable<ProductImage> query = _context.ProductImages.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            return await query.FirstOrDefaultAsync();
        }
    }
}
