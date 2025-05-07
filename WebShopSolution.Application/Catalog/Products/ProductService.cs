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
        private readonly IUnitOfWork _unitOfWork;
        private readonly WebShopDbContext _context;

        public ProductService(IUnitOfWork unitOfWork, WebShopDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // Lấy tất cả sản phẩm
        public async Task<List<ProductViewModel>> GetAllAsync()
        {
            var products = await _context.Products
                                          .Include(p => p.ProductImages)
                                          .Include(p => p.Variants)
                                          .ToListAsync();

            if (products == null || !products.Any())
            {
                return new List<ProductViewModel>(); // Trả về danh sách rỗng
            }

            return products.Select(p => new ProductViewModel
            {
                IdProduct = p.IdProduct,
                ProductName = p.ProductName,
                Description = p.Description,
                BasePrice = p.BasePrice,
                Quantity = p.Quantity,
                CategoryName = p.Category != null ? p.Category.CateName : "Unknown",
                // Kiểm tra null và ánh xạ đúng
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
                    Price = v.Price
                }).ToList() ?? new List<ProductVariantViewModel>()
            }).ToList();
        }


        // Lấy chi tiết sản phẩm theo ID
        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
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

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(); // Commit changes to the database
        }

        // Cập nhật sản phẩm
        public async Task UpdateAsync(ProductUpdateRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.IdProduct);
            if (product == null) return;

            product.ProductName = request.ProductName;
            product.Description = request.Description;
            product.BasePrice = request.BasePrice;
            product.Quantity = request.Quantity;
            product.IdCate = request.CategoryId;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(); // Commit changes to the database
        }

        // Xóa sản phẩm
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(); // Commit changes to the database
        }
    }
}
