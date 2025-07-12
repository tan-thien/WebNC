using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Voucher;

namespace WebShopSolution.Application.Catalog.Vouchers
{
    public interface IVoucherService
    {
        Task<List<Voucher>> GetAllAsync();
        Task<Voucher> GetByIdAsync(int id);
        Task<bool> CreateAsync(VoucherCreateRequest request);
        Task<bool> UpdateAsync(VoucherUpdateRequest request);
        Task<bool> DeleteAsync(int id);
        Task<List<VoucherViewModel>> GetAvailableVouchersAsync();
        Task<VoucherApplyResult> PreviewVoucherDiscountAsync(string code, int totalAmount);

    }
}
