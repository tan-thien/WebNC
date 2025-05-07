using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.Application.Catalog.ProductVariant
{
    public class ProductVariantAttributeService : IProductVariantAttributeService
    {
        private readonly WebShopDbContext _context;

        public ProductVariantAttributeService(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ProductVariantAttributeCreateRequest request)
        {
            var attribute = new ProductVariantAttribute
            {
                AttributeName = request.AttributeName,
                AttributeValue = request.AttributeValue
            };

            _context.ProductVariantAttributes.Add(attribute);
            await _context.SaveChangesAsync();

            return attribute.Id;
        }
    }


}
