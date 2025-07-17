using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.CartItem;
using WebShopSolution.ViewModels.Catalog.Order;

namespace WebShopSolution.Application.Catalog.Orders
{
    public interface IOrderService
    {
        //user
        Task<bool> CreateOrderAsync(int userId, OrderCreateRequest request);
        Task<List<OrderViewModel>> GetOrdersByUserIdAsync(int userId);
        //admin
        Task<List<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderViewModel?> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(OrderStatusUpdateRequest request);
        Task<(bool IsValid, int DiscountAmount, int OriginalTotal, string Message, int? VoucherId, int? CustomerVoucherId)>
ApplyVoucherAsync(string voucherCode, int originalTotal, int userId);

        Task<bool> UpdateStockAfterOrderAsync(List<CartItemSelectionRequest> items);

    }
}
