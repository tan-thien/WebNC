using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);

        // ✅ Sửa tên tránh trùng overload
        Task<IEnumerable<Order>> GetWithIncludeAsync(
            Expression<Func<Order, bool>> predicate,
            string includeProperties = "");
    }
}
