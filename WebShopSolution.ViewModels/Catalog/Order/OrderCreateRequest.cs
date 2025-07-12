using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.CartItem;

namespace WebShopSolution.ViewModels.Catalog.Order
{
    public class OrderCreateRequest
    {
        public string ProductName { get; set; }
        public string VariantInfo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        public string? VoucherCode { get; set; }
        public int TotalAmount { get; set; }
        public int DiscountAmount { get; set; }  // Số tiền được giảm
        public bool IsVoucherFromCart { get; set; }  // Đánh dấu mã đến từ Cart hay nhập ở Checkout
        public List<CartItemSelectionRequest> Items { get; set; }
    }

}
