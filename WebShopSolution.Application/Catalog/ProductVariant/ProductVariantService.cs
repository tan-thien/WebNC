using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Product;
using Microsoft.EntityFrameworkCore;

namespace WebShopSolution.Application.Catalog.ProductVariant
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly WebShopDbContext _context;

        public ProductVariantService(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariantViewModel> CreateAsync(ProductVariantCreateRequest request)
        {
            var variant = new Data.Entities.ProductVariant
            {
                ProductId = request.ProductId,
                Sku = request.Sku,
                Price = request.Price,
                Stock = request.Stock,
                Attributes = request.Attributes.Select(a => new ProductVariantAttribute
                {
                    AttributeName = a.AttributeName,
                    AttributeValue = a.AttributeValue
                }).ToList()
            };

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            return new ProductVariantViewModel
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                Sku = variant.Sku,
                Price = variant.Price,
                Stock = variant.Stock,
                Attributes = variant.Attributes.Select(a => new ProductVariantAttributeViewModel
                {
                    Id = a.Id,
                    AttributeName = a.AttributeName,
                    AttributeValue = a.AttributeValue
                }).ToList()
            };
        }


        public async Task<List<ProductVariantViewModel>> GetByProductIdAsync(int productId)
        {
            var variants = await _context.ProductVariants
                .Where(x => x.ProductId == productId)
                .Include(x => x.Attributes)
                .ToListAsync();

            return variants.Select(v => new ProductVariantViewModel
            {
                Id = v.Id,
                ProductId=v.ProductId,
                Sku = v.Sku,
                Price = v.Price,
                Stock = v.Stock,
                Attributes = v.Attributes.Select(a => new ProductVariantAttributeViewModel
                {
                    Id = a.Id,
                    AttributeName = a.AttributeName,
                    AttributeValue = a.AttributeValue
                }).ToList()
            }).ToList();
        }
    }
}
