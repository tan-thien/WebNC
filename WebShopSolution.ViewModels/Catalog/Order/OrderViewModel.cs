using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.OrderDetail;

namespace WebShopSolution.ViewModels.Catalog.Order
{
    public class OrderViewModel
    {
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string OrderStatus { get; set; }  // 🔄 Đã sửa tên
        public string ShippingAddress { get; set; }
        public string Phone { get; set; }
        public int TotalAmount { get; set; }
        public string VoucherCode { get; set; }   
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int DiscountAmount { get; set; } // Số tiền giảm giá
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

}
