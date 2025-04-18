using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Application.Catalog.Orders
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
