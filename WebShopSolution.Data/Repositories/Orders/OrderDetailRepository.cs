using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Orders
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly WebShopDbContext _context;

        public OrderDetailRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }
        public async Task<List<OrderDetail>> GetDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(d => d.OrderId == orderId)
                .ToListAsync();
        }
    }
}
