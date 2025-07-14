using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;

namespace WebShopSolution.Application.Catalog.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _unitOfWork.Customers.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Customers.GetByIdAsync(id);
        }

        public async Task<Customer?> GetByAccountIdAsync(int accountId)
        {
            return await _unitOfWork.Customers.GetByAccountIdAsync(accountId);
        }

        public async Task AddAsync(Customer customer)
        {
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _unitOfWork.Customers.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Customers.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(int accountId, string name, string phone, string address, string? newPassword = null, string? username = null)
        {
            var customer = await _unitOfWork.Customers.GetByAccountIdAsync(accountId);
            if (customer == null || customer.Account == null) return;

            customer.CusName = name;
            customer.Phone = phone;
            customer.Address = address;

            if (!string.IsNullOrWhiteSpace(username))
            {
                customer.Account.UserName = username;
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                customer.Account.PassWord = newPassword;
                await _unitOfWork.Accounts.UpdateAsync(customer.Account);
            }

            await _unitOfWork.Customers.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
