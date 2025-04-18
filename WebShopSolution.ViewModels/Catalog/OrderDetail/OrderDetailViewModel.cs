using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.OrderDetail
{
    public class OrderDetailViewModel
    {
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }

}
