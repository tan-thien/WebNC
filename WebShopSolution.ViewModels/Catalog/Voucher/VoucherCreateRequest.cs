using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Voucher
{
    public class VoucherCreateRequest
    {
        public string Mode { get; set; } = "manual";         // "manual" hoặc "preset"
        public string? PresetName { get; set; }              // Nếu chọn preset

        public string? Code { get; set; }
        public bool AutoGenerateCode { get; set; } = true;
        public int CodeLength { get; set; } = 8;

        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte DiscountType { get; set; }
        public int DiscountValue { get; set; }
        public int? MaxDiscountValue { get; set; }
        public int Quantity { get; set; }
        public int UserLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
