using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.Repositories.Vouchers
{
    public class CustomerVoucherRepository : ICustomerVoucherRepository
    {
        private readonly WebShopDbContext _context;

        public CustomerVoucherRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerVoucher?> GetByIdAsync(int id)
        {
            return await _context.CustomerVouchers.FindAsync(id);
        }

        public async Task<List<CustomerVoucher>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerVouchers
                .Where(cv => cv.IdCus == customerId)
                .ToListAsync();
        }

        public async Task AddAsync(CustomerVoucher entity)
        {
            await _context.CustomerVouchers.AddAsync(entity);
        }

        public async Task UpdateAsync(CustomerVoucher entity)
        {
            _context.CustomerVouchers.Update(entity);
        }
        public async Task<CustomerVoucher?> GetFirstOrDefaultAsync(Expression<Func<CustomerVoucher, bool>> predicate)
        {
            return await _context.CustomerVouchers.FirstOrDefaultAsync(predicate);
        }
    }
}
