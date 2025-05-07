/*using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        [ForeignKey("Customer")]
        public int IdCus { get; set; }
        public Customer Customer { get; set; }
        public string StatusOrder { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string? MoTa { get; set; }
        public int TongTien { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public string PaymentMethodName { get; set; }

    }
}*/
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

        public string StatusOrder { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int TotalAmount { get; set; }

        public string? OrderStatus { get; set; }  // Ví dụ: Đang xử lý, Đã giao, Hủy...

        // Foreign key to Customer (mỗi đơn hàng thuộc về một khách hàng)
        [ForeignKey("Customer")]
        public int IdCus { get; set; }
        public Customer Customer { get; set; }

        // Navigation properties
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
