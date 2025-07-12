using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        // 🔹 Thêm khóa ngoại tới ProductVariant
        public int? VariantId { get; set; } // Nullable nếu không phải sp nào cũng có biến thể
        [ForeignKey("VariantId")]
        public ProductVariant ProductVariant { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; } // Số lượng tồn kho
        public string VariantInfo { get; set; } // Lưu thông tin biến thể (VD: "màu: đỏ, kích cỡ: M")
    }
}
