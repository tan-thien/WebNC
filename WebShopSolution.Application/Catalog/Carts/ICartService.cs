using System.Collections.Generic;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.CartItem;

namespace WebShopSolution.Application.Catalog.Carts
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(CartItemCreateRequest request);
        List<CartItemViewModel> GetCartItems(int cartId);
        Task<int> GetOrCreateCartIdByUserId(int userId);
        Task<bool> UpdateQuantityAsync(int cartId, int productId, int? variantId, int quantity);
        Task<bool> DeleteItemAsync(int cartId, int productId, int? variantId);
        Task<ProductVariant?> GetProductVariantByIdAsync(int variantId);

        Task DeleteItemsFromCartAsync(int cartId, List<CartItemSelectionRequest> items);

    }
}
