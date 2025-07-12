using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WebShopSolution.Data.Repositories.Vouchers
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly WebShopDbContext _context;

        public VoucherRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Voucher>> GetAllAsync()
            => await _context.Vouchers.ToListAsync();

        public async Task<Voucher> GetByIdAsync(int id)
            => await _context.Vouchers.FindAsync(id);

        public async Task AddAsync(Voucher voucher)
        {
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Voucher?> GetFirstOrDefaultAsync(Expression<Func<Voucher, bool>> predicate)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(predicate);
        }
    }

}
