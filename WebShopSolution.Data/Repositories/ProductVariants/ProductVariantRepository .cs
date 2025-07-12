using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.ProductVariants
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly WebShopDbContext _context;

        public ProductVariantRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariant?> GetByIdAsync(int id)
        {
            return await _context.ProductVariants.FindAsync(id);
        }
        public async Task<IEnumerable<ProductVariant>> GetWithIncludeAsync(
            Expression<Func<ProductVariant, bool>> predicate,
            string includeProperties = "")
        {
            IQueryable<ProductVariant> query = _context.ProductVariants.Where(predicate);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return await query.ToListAsync();
        }
    }
}
