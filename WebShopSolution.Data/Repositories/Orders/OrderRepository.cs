using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebShopDbContext _context;

        public OrderRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.IdCus == customerId)
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Order>> GetWithIncludeAsync(
            Expression<Func<Order, bool>> predicate,
            string includeProperties = "")
                {
            IQueryable<Order> query = _context.Orders.Where(predicate);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(
                    new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return await query.ToListAsync();
        }
    }
}
