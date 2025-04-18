using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class CartItemCreateRequest
    {
        [Required]
        public int IdCart { get; set; }

        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
