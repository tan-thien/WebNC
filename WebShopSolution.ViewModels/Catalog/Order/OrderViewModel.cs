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
        public string StatusOrder { get; set; }
        public string ShippingAddress { get; set; }
        public string Phone { get; set; }
        public int TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

}
