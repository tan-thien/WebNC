using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Application.Catalog.Accounts
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int id);
        Task<Account?> GetByUsernameAsync(string username);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id);
    }
}
