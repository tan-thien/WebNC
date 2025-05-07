using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.OrderDetail
{
    public class OrderDetailViewModel
    {
        public int IdOrderDetail { get; set; }
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice => Quantity * Price;
    }


}
