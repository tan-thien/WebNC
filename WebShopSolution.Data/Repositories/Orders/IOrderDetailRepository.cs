using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Orders
{
    public interface IOrderDetailRepository
    {
        Task AddAsync(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetDetailsByOrderIdAsync(int orderId);

    }
}
