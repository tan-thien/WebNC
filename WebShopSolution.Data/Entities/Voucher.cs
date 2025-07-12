using System;
using System.ComponentModel.DataAnnotations;

namespace WebShopSolution.Data.Entities
{
    public class Voucher
    {
        [Key]
        public int VoucherId { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } // Auto generate hoặc nhập

        // 0 = Giảm tiền cố định, 1 = Giảm %
        public byte DiscountType { get; set; }

        public int DiscountValue { get; set; } // dùng int thay vì decimal

        // Chỉ áp dụng nếu là giảm %
        public int? MaxDiscountValue { get; set; }

        public int Quantity { get; set; } = 1; // Số lượt phát hành

        public int Used { get; set; } = 0; // Hệ thống tự cập nhật khi dùng

        public int UserLimit { get; set; } = 1; // Số lần mỗi khách được dùng

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Optional
        [MaxLength(100)]
        public string? Name { get; set; } // Có thể sinh tự động

        public string? Description { get; set; } // Có thể ẩn luôn nếu không cần

        // Nếu muốn đơn giản hóa hơn nữa, có thể bỏ Name + Description

        // --- Navigation ---
        public ICollection<CustomerVoucher> CustomerVouchers { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

}
