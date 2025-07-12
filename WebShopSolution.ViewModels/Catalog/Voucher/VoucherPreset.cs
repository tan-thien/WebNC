using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Voucher
{
    public class VoucherPreset
    {
        public string Name { get; set; }              // Tên hiển thị
        public string Code { get; set; }              // Mã code cố định
        public string Description { get; set; }       // Mô tả
        public byte DiscountType { get; set; }        // 0: giảm tiền, 1: giảm %
        public int DiscountValue { get; set; }
        public int? MaxDiscountValue { get; set; }
        public int Quantity { get; set; } = 100;
        public int UserLimit { get; set; } = 1;
        public int DurationDays { get; set; } = 30;

        public static List<VoucherPreset> GetPresets()
        {
            return new List<VoucherPreset>
            {
                new VoucherPreset {
                    Name = "FREESHIP50K",
                    Code = "FREESHIP50K",
                    Description = "Giảm 50K cho đơn từ 500K",
                    DiscountType = 0,
                    DiscountValue = 50000,
                    MaxDiscountValue = null,
                    Quantity = 100,
                    UserLimit = 1,
                    DurationDays = 30
                },
                new VoucherPreset {
                    Name = "GIAO_MIEN_PHI",
                    Code = "GIAO_MIEN_PHI",
                    Description = "Giảm 30K như freeship",
                    DiscountType = 0,
                    DiscountValue = 30000,
                    MaxDiscountValue = null,
                    Quantity = 100,
                    UserLimit = 1,
                    DurationDays = 30
                },
                new VoucherPreset {
                    Name = "SALE10PERCENT",
                    Code = "SALE10PERCENT",
                    Description = "Giảm 10% tối đa 100K",
                    DiscountType = 1,
                    DiscountValue = 10,
                    MaxDiscountValue = 100000,
                    Quantity = 200,
                    UserLimit = 1,
                    DurationDays = 30
                },
                new VoucherPreset {
                    Name = "DANG_KY_MOI",
                    Code = "DANG_KY_MOI",
                    Description = "Giảm 20K cho khách đăng ký mới",
                    DiscountType = 0,
                    DiscountValue = 20000,
                    MaxDiscountValue = null,
                    Quantity = 500,
                    UserLimit = 1,
                    DurationDays = 30
                },
                new VoucherPreset {
                    Name = "TUAN_LE_VANG",
                    Code = "TUAN_LE_VANG",
                    Description = "Giảm 15% tất cả sản phẩm",
                    DiscountType = 1,
                    DiscountValue = 15,
                    MaxDiscountValue = 120000,
                    Quantity = 300,
                    UserLimit = 1,
                    DurationDays = 7
                }
            };
        }
    }
}
