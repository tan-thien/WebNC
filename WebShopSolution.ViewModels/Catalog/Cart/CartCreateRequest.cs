using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Cart
{
    public class CartCreateRequest
    {
        [Required]
        public int IdCus { get; set; }
    }
}
