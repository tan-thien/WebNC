using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Account;

namespace WebShopSolution.Application.Catalog.Accounts
{
    public interface IAccountService
    {
        Task<int?> RegisterAsync(AccountCreateRequest request);
        Task<AccountViewModel?> LoginAsync(AccountLoginRequest request);
        Task<List<AccountViewModel>> GetAllAsync();
        Task<bool> UpdateAsync(AccountUpdateRequest request);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);


        Task<int?> CreateAsync(AccountCreateRequest request);
        Task<bool> ChangeRoleAsync(int idAcc, string newRole);

    }

}
