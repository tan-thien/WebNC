using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.OrderDetail
{
    public class OrderDetailCreateRequest
    {
        [Required]
        public int IdOrder { get; set; }

        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }
    }

}
