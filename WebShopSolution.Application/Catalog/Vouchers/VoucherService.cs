using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.Repositories.Vouchers;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Voucher;

namespace WebShopSolution.Application.Catalog.Vouchers
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public VoucherService(
            IVoucherRepository repository,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Voucher>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Voucher> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        //admin
        public async Task<bool> CreateAsync(VoucherCreateRequest request)
        {
            // Nếu chọn chế độ preset
            if (request.Mode == "preset" && !string.IsNullOrEmpty(request.PresetName))
            {
                var preset = VoucherPreset.GetPresets().FirstOrDefault(p => p.Name == request.PresetName);
                if (preset != null)
                {
                    request.Name = preset.Name;
                    request.Description = preset.Description;
                    request.DiscountType = preset.DiscountType;
                    request.DiscountValue = preset.DiscountValue;
                    request.MaxDiscountValue = preset.MaxDiscountValue;
                    request.Quantity = preset.Quantity;
                    request.UserLimit = preset.UserLimit;
                    request.StartDate = DateTime.Today;
                    request.EndDate = DateTime.Today.AddDays(preset.DurationDays);
                    request.Code = preset.Code;
                    request.AutoGenerateCode = false;
                }
            }

            string code = request.Code;
            if (request.AutoGenerateCode || string.IsNullOrWhiteSpace(code))
            {
                bool isDuplicate;
                do
                {
                    code = GenerateRandomCode(request.CodeLength);
                    isDuplicate = (await _repository.GetAllAsync()).Any(v => v.Code == code);
                } while (isDuplicate);
            }

            var voucher = new Voucher
            {
                Code = code,
                Name = request.Name,
                Description = request.Description,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                MaxDiscountValue = request.MaxDiscountValue,
                Quantity = request.Quantity,
                UserLimit = request.UserLimit,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                Used = 0
            };

            await _repository.AddAsync(voucher);
            return true;
        }


        public async Task<bool> UpdateAsync(VoucherUpdateRequest request)
        {
            var voucher = await _repository.GetByIdAsync(request.VoucherId);
            if (voucher == null) return false;

            voucher.Code = request.Code;
            voucher.Name = request.Name;
            voucher.Description = request.Description;
            voucher.DiscountType = request.DiscountType;
            voucher.DiscountValue = request.DiscountValue;
            voucher.MaxDiscountValue = request.MaxDiscountValue;
            voucher.Quantity = request.Quantity;
            voucher.UserLimit = request.UserLimit;
            voucher.StartDate = request.StartDate;
            voucher.EndDate = request.EndDate;
            voucher.IsActive = request.IsActive;

            await _repository.UpdateAsync(voucher);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<List<VoucherViewModel>> GetAvailableVouchersAsync()
        {
            var now = DateTime.Now;
            var vouchers = await _repository.GetAllAsync();

            var availableVouchers = vouchers
                .Where(v => v.IsActive
                    && v.StartDate <= now
                    && v.EndDate >= now
                    && v.Quantity > v.Used)
                .Select(v => new VoucherViewModel
                {
                    VoucherId = v.VoucherId,
                    Code = v.Code,
                    Name = v.Name,
                    Description = v.Description,
                    DiscountType = v.DiscountType,
                    DiscountValue = v.DiscountValue,
                    MaxDiscountValue = v.MaxDiscountValue,
                    Quantity = v.Quantity,
                    Used = v.Used,
                    UserLimit = v.UserLimit,
                    StartDate = v.StartDate,
                    EndDate = v.EndDate,
                    IsActive = v.IsActive
                })
                .ToList();

            return availableVouchers;
        }


        //user( ben gio hang)
        public async Task<VoucherApplyResult> PreviewVoucherDiscountAsync(string code, int totalAmount)
        {
            var voucher = (await _repository.GetAllAsync())
                .FirstOrDefault(v => v.Code == code
                    && v.IsActive
                    && v.StartDate <= DateTime.Now
                    && v.EndDate >= DateTime.Now
                    && v.Quantity > v.Used);

            if (voucher == null)
            {
                return new VoucherApplyResult
                {
                    IsValid = false,
                    Message = "Mã giảm giá không hợp lệ hoặc đã hết hạn."
                };
            }

            // Lấy userId từ session
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.Session.GetInt32("UserId");
            if (userId == null)
            {
                return new VoucherApplyResult
                {
                    IsValid = false,
                    Message = "Bạn cần đăng nhập để áp dụng mã."
                };
            }

            // Tìm khách hàng từ userId
            var customer = await _unitOfWork.Customers.GetByAccountIdAsync(userId.Value);
            if (customer == null)
            {
                return new VoucherApplyResult
                {
                    IsValid = false,
                    Message = "Không tìm thấy thông tin khách hàng."
                };
            }

            // Kiểm tra mã đã tồn tại với khách chưa
            var customerVoucher = await _unitOfWork.CustomerVouchers
                .GetFirstOrDefaultAsync(x => x.VoucherId == voucher.VoucherId && x.IdCus == customer.IdCus);

            if (customerVoucher == null)
            {
                // Nếu chưa có thì thêm vào
                customerVoucher = new CustomerVoucher
                {
                    IdCus = customer.IdCus,
                    VoucherId = voucher.VoucherId,
                    IsUsed = false,
                    UsedAt = null
                };
                await _unitOfWork.CustomerVouchers.AddAsync(customerVoucher);
                await _unitOfWork.SaveChangesAsync();
            }
            else if (customerVoucher.IsUsed)
            {
                return new VoucherApplyResult
                {
                    IsValid = false,
                    Message = "Bạn đã sử dụng mã này rồi."
                };
            }

            // Tính giảm giá
            int discountAmount = 0;
            if (voucher.DiscountType == 0)
            {
                discountAmount = voucher.DiscountValue;
            }
            else if (voucher.DiscountType == 1)
            {
                discountAmount = totalAmount * voucher.DiscountValue / 100;
            }

            if (voucher.MaxDiscountValue > 0 && discountAmount > voucher.MaxDiscountValue)
            {
                discountAmount = voucher.MaxDiscountValue ?? 0;
            }

            return new VoucherApplyResult
            {
                IsValid = true,
                DiscountAmount = discountAmount,
                OriginalTotal = totalAmount,
                Message = "Áp dụng mã thành công."
            };
        }

    }


}
