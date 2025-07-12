using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Voucher
{
    public class VoucherViewModel
    {
        public int VoucherId { get; set; }

        public string Code { get; set; } = string.Empty;

        public byte DiscountType { get; set; }          // 0: giảm tiền, 1: giảm %
        public int DiscountValue { get; set; }

        public int? MaxDiscountValue { get; set; }

        public int Quantity { get; set; }
        public int Used { get; set; }
        public int UserLimit { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        // 👇 Tự động format hiển thị kiểu ưu đãi
        public string DiscountText
        {
            get
            {
                if (DiscountType == 0)
                {
                    return $"{DiscountValue:N0}₫";
                }
                else
                {
                    string maxPart = MaxDiscountValue.HasValue ? $" (tối đa {MaxDiscountValue.Value:N0}₫)" : "";
                    return $"{DiscountValue}%{maxPart}";
                }
            }
        }

        // 👇 Kiểm tra trạng thái còn hiệu lực
        public bool IsAvailableNow
        {
            get
            {
                var now = DateTime.Now;
                return IsActive && StartDate <= now && EndDate >= now && Quantity > Used;
            }
        }
    }
}
