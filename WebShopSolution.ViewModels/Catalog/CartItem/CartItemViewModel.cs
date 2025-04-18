using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class CartItemViewModel
    {
        public int IdCart { get; set; }
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
