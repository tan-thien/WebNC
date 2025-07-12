using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class SaveSelectedCartRequest
    {
        public List<CartItemViewModel> Items { get; set; }
        public int TotalAmount { get; set; }
        public string? VoucherCode { get; set; }

    }
}
