using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductCreateRequest
    {
        [Required]
        public string ProductName { get; set; }

        public string? Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public int IdCate { get; set; }
    }

}
