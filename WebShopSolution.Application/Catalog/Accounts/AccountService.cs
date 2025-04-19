using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Account;

namespace WebShopSolution.Application.Catalog.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> RegisterAsync(AccountCreateRequest request)
        {
            var existing = await _unitOfWork.Accounts.GetByUsernameAsync(request.UserName);
            if (existing != null) return false;

            var account = new Account
            {
                UserName = request.UserName,
                PassWord = request.PassWord, // Gợi ý: Mã hoá mật khẩu ở đây nếu cần
                Status = request.Status,
                Role = request.Role
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<AccountViewModel?> LoginAsync(AccountLoginRequest request)
        {
            var account = await _unitOfWork.Accounts.GetByUsernameAsync(request.UserName);
            if (account == null || account.PassWord != request.PassWord)
                return null;

            return new AccountViewModel
            {
                IdAcc = account.IdAcc,
                UserName = account.UserName,
                Role = account.Role,
                Status = account.Status ?? ""
            };
        }

        public async Task<List<AccountViewModel>> GetAllAsync()
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync();
            return accounts.Select(acc => new AccountViewModel
            {
                IdAcc = acc.IdAcc,
                UserName = acc.UserName,
                Role = acc.Role,
                Status = acc.Status ?? ""
            }).ToList();
        }

        public async Task<bool> UpdateAsync(AccountUpdateRequest request)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.IdAcc);
            if (account == null) return false;

            account.Role = request.Role;
            account.Status = request.Status;

            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id);
            if (account == null) return false;

            await _unitOfWork.Accounts.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.IdAcc);
            if (account == null || account.PassWord != request.CurrentPassword)
                return false;

            account.PassWord = request.NewPassword;
            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
