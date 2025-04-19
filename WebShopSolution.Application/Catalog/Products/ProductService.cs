using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Product;

namespace WebShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Sử dụng phương thức GetAllWithCategoryAsync để lấy tất cả sản phẩm với Category
        public async Task<List<ProductViewModel>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            return products.Select(p => new ProductViewModel
            {
                IdProduct = p.IdProduct,
                ProductName = p.ProductName,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                Size = p.Size,
                Image = p.Image,
                IdCate = p.IdCate,
                CategoryName = p.Category?.CateName // Truyền thông tin danh mục
            }).ToList();
        }

        // Sử dụng phương thức GetByIdWithCategoryAsync để lấy sản phẩm theo id với Category
        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            if (product == null) return null;

            return new ProductViewModel
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Size = product.Size,
                Image = product.Image,
                IdCate = product.IdCate,
                CategoryName = product.Category?.CateName // Truyền thông tin danh mục
            };
        }


        public async Task<int> CreateAsync(ProductCreateRequest request)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Quantity = request.Quantity,
                Price = request.Price,
                Size = request.Size,
                Image = request.Image,
                IdCate = request.IdCate
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product.IdProduct;
        }

        public async Task<bool> UpdateAsync(ProductUpdateRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.IdProduct);
            if (product == null) return false;

            product.ProductName = request.ProductName;
            product.Description = request.Description;
            product.Quantity = request.Quantity;
            product.Price = request.Price;
            product.Size = request.Size;
            product.Image = request.Image;
            product.IdCate = request.IdCate;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
