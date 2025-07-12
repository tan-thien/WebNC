using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Vouchers
{
    public interface ICustomerVoucherRepository 
    {
        Task<CustomerVoucher?> GetByIdAsync(int id);
        Task<List<CustomerVoucher>> GetByCustomerIdAsync(int customerId);
        Task<CustomerVoucher?> GetFirstOrDefaultAsync(Expression<Func<CustomerVoucher, bool>> predicate);

        Task AddAsync(CustomerVoucher entity);
        Task UpdateAsync(CustomerVoucher entity);
    }
}
