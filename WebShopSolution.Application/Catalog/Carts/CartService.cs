using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.CartItem;

namespace WebShopSolution.Application.Catalog.Carts
{
    public class CartService : ICartService
    {
        private readonly WebShopDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(WebShopDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddToCartAsync(CartItemCreateRequest request)
        {
            try
            {
                Console.WriteLine($"👉 BẮT ĐẦU AddToCartAsync - CartId: {request.IdCart}, ProductId: {request.IdProduct}, VariantId: {request.VariantId}");

                var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == request.IdCart);
                if (cart == null) return false;

                var product = await _context.Products.FindAsync(request.IdProduct);
                if (product == null) return false;

                string variantInfo = string.Empty;
                int finalPrice = product.BasePrice ?? 0;
                string imagePath = _context.ProductImages.FirstOrDefault(i => i.ProductId == product.IdProduct)?.ImagePath ?? "";

                if (request.VariantId.HasValue)
                {
                    var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == request.VariantId.Value);
                    if (variant == null) return false;

                    finalPrice = variant.Price > 0 ? variant.Price : finalPrice;

                    var attributes = await _context.ProductVariantAttributes
                        .Where(a => a.VariantId == variant.Id)
                        .OrderBy(a => a.AttributeName)
                        .Select(a => $"{a.AttributeName}: {a.AttributeValue}")
                        .ToListAsync();

                    variantInfo = string.Join(", ", attributes);
                }

                var existingItem = await _context.CartItems.FirstOrDefaultAsync(ci =>
                    ci.CartId == request.IdCart &&
                    ci.ProductId == request.IdProduct &&
                    (
                        (ci.VariantId == null && request.VariantId == null) ||
                        (ci.VariantId != null && ci.VariantId == request.VariantId)
                    ));

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartId = request.IdCart,
                        ProductId = request.IdProduct,
                        Quantity = request.Quantity,
                        ProductName = product.ProductName,
                        ImageUrl = imagePath,
                        Price = finalPrice,
                        VariantId = request.VariantId,
                        VariantInfo = variantInfo,
                        Stock = product.Quantity ?? 0
                    };
                    _context.CartItems.Add(newItem);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<CartItemViewModel> GetCartItems(int cartId)
        {
            var items = _context.CartItems.Where(x => x.CartId == cartId).ToList();
            return items.Select(x => new CartItemViewModel
            {
                IdCart = x.CartId,
                IdProduct = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                Attributes = x.VariantInfo,
                ImagePath = x.ImageUrl,
                Price = x.Price,
                VariantId = x.VariantId,
                VariantSummary = x.VariantInfo
            }).ToList();
        }

        public async Task<int> GetOrCreateCartIdByUserId(int userId)
        {
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.IdAcc == userId);
            if (existingCart != null) return existingCart.Id;

            var newCart = new Cart { IdAcc = userId };
            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();
            return newCart.Id;
        }

        public async Task<bool> UpdateQuantityAsync(int cartId, int productId, int? variantId, int quantity)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.ProductId == productId &&
                (
                    (variantId == null && ci.VariantId == null) ||
                    (variantId != null && ci.VariantId == variantId)
                ));

            if (item == null) return false;

            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(int cartId, int productId, int? variantId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.ProductId == productId &&
                (
                    (variantId == null && ci.VariantId == null) ||
                    (variantId != null && ci.VariantId == variantId)
                ));

            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ProductVariant?> GetProductVariantByIdAsync(int variantId)
        {
            return await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        }
    }
}
