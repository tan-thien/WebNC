using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class CartDeleteRequest
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
    }

}
