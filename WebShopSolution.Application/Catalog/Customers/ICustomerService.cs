using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Application.Catalog.Customers
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByAccountIdAsync(int accountId);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}
