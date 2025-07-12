using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Vouchers
{
    public interface IVoucherRepository
    {
        Task<List<Voucher>> GetAllAsync();
        Task<Voucher> GetByIdAsync(int id);
        Task AddAsync(Voucher voucher);
        Task UpdateAsync(Voucher voucher);
        Task DeleteAsync(int id);
        Task<Voucher?> GetFirstOrDefaultAsync(Expression<Func<Voucher, bool>> predicate);
    }
}
