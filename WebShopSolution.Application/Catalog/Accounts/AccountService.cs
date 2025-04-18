using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;

namespace WebShopSolution.Application.Catalog.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _unitOfWork.Accounts.GetAllAsync();
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Accounts.GetByIdAsync(id);
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _unitOfWork.Accounts.GetByUsernameAsync(username);
        }

        public async Task AddAsync(Account account)
        {
            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Accounts.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
