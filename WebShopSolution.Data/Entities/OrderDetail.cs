/*using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int IdOrder { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int IdProduct { get; set; }
        public Product Product { get; set; }

        public int SoLuong { get; set; }
        public int Price { get; set; }
    }
}
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSolution.Data.Entities
{
    public class OrderDetail
    {
        [Key]
        public int IdOrderDetail { get; set; }

        // Foreign Key to Order (mỗi chi tiết đơn hàng thuộc về một đơn hàng)
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Có thể có hoặc không có ProductVariant
        [ForeignKey("Product")]
        public int ProductId { get; set; } // Trỏ đến Product nếu không có Variant

        public Product Product { get; set; } // Navigation property đến Product


        // Foreign Key to ProductVariant (sản phẩm thuộc về một biến thể)
        [ForeignKey("ProductVariant")]
        public int? VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        // Tính tổng giá trị chi tiết đơn hàng
        public int TotalPrice => Quantity * Price;
    }
}
