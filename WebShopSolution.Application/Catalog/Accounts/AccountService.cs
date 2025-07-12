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

        public async Task<int?> RegisterAsync(AccountCreateRequest request)
        {
            var existing = await _unitOfWork.Accounts.GetByUsernameAsync(request.UserName);
            if (existing != null) return null; // username đã tồn tại

            var account = new Account
            {
                UserName = request.UserName,
                PassWord = request.PassWord, // Bạn nhớ mã hóa mật khẩu nhé
                Status = request.Status ?? "Active",
                Role = request.Role ?? "User"
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return account.IdAcc; // trả về Id tài khoản mới tạo
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



        public async Task<int?> CreateAsync(AccountCreateRequest request)
        {
            var existing = await _unitOfWork.Accounts.GetByUsernameAsync(request.UserName);
            if (existing != null) return null; // Tài khoản đã tồn tại

            var account = new Account
            {
                UserName = request.UserName,
                PassWord = request.PassWord, // Nên mã hóa
                Status = request.Status ?? "Active",
                Role = request.Role ?? "User"
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            // Nếu là User thì thêm Customer
            if (account.Role == "User")
            {
                var customer = new Customer
                {
                    CusName = request.CusName,
                    Phone = request.Phone,
                    Address = request.Address,
                    IdAcc = account.IdAcc
                };
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();
            }

            return account.IdAcc;
        }
        public async Task<bool> ChangeRoleAsync(int idAcc, string newRole)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(idAcc);
            if (account == null) return false;

            account.Role = newRole;
            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


    }
}
