using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;

namespace WebShopSolution.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _unitOfWork.Orders.GetAllAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _unitOfWork.Orders.GetByCustomerIdAsync(customerId);
        }

        public async Task AddAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
