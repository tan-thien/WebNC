using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly WebShopDbContext _context;

        public ProductService(WebShopDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả sản phẩm
        public async Task<List<ProductViewModel>> GetAllAsync()
        {
            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .Include(p => p.ProductImages)
                                         .Include(p => p.Variants)
                                             .ThenInclude(v => v.Attributes)
                                         .ToListAsync();

            return products.Select(p => new ProductViewModel
            {
                IdProduct = p.IdProduct,
                ProductName = p.ProductName,
                Description = p.Description,
                BasePrice = p.BasePrice,
                Quantity = p.Quantity,
                IdCate = p.IdCate,
                CategoryName = p.Category?.CateName ?? "Unknown",
                ProductImages = p.ProductImages?.Select(pi => new ProductImageViewModel
                {
                    Id = pi.Id,
                    ImagePath = pi.ImagePath,
                    Caption = pi.Caption,
                    SortOrder = pi.SortOrder,
                    IsDefault = pi.IsDefault
                }).ToList() ?? new List<ProductImageViewModel>(),

                Variants = p.Variants?.Select(v => new ProductVariantViewModel
                {
                    Id = v.Id,
                    Sku = v.Sku,
                    Stock = v.Stock,
                    Price = v.Price,
                    Attributes = v.Attributes.Select(a => new ProductVariantAttributeViewModel
                    {
                        Id = a.Id,
                        AttributeName = a.AttributeName,
                        AttributeValue = a.AttributeValue
                    }).ToList()
                }).ToList() ?? new List<ProductVariantViewModel>()
            }).ToList();
        }

        // Lấy chi tiết sản phẩm theo ID
        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.ProductImages)
                                        .Include(p => p.Variants)
                                            .ThenInclude(v => v.Attributes)
                                        .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null) return null;

            return new ProductViewModel
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Description = product.Description,
                BasePrice = product.BasePrice,
                Quantity = product.Quantity,
                CategoryName = product.Category?.CateName,
                ProductImages = product.ProductImages.Select(img => new ProductImageViewModel
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath
                }).ToList(),
                Variants = product.Variants.Select(v => new ProductVariantViewModel
                {
                    Id = v.Id,
                    Sku = v.Sku,
                    Price = v.Price,
                    Stock = v.Stock,
                    Attributes = v.Attributes.Select(a => new ProductVariantAttributeViewModel
                    {
                        AttributeName = a.AttributeName,
                        AttributeValue = a.AttributeValue
                    }).ToList()
                }).ToList()
            };
        }

        // Thêm mới sản phẩm
        public async Task AddAsync(ProductCreateRequest request)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                BasePrice = request.BasePrice,
                Quantity = request.Quantity,
                IdCate = request.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Cập nhật sản phẩm
        public async Task UpdateAsync(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.IdProduct);
            if (product == null) return;

            product.ProductName = request.ProductName;
            product.Description = request.Description;
            product.BasePrice = request.BasePrice;
            product.Quantity = request.Quantity;
            product.IdCate = request.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

}
