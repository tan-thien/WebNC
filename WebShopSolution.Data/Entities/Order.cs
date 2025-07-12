using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSolution.Data.Entities
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? NgayGiao { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int TotalAmount { get; set; }

        public string? OrderStatus { get; set; }

        [ForeignKey("Customer")]
        public int IdCus { get; set; }
        public Customer Customer { get; set; }

        // ✅ Thêm voucher sử dụng
        [ForeignKey("Voucher")]
        public int? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }

        // ✅ Thêm thông tin sử dụng mã cụ thể của khách hàng
        [ForeignKey("CustomerVoucher")]
        public int? CustomerVoucherId { get; set; }
        public CustomerVoucher? CustomerVoucher { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
