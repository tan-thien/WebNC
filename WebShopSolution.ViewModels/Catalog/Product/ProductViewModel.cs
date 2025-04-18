using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductViewModel
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
        public int IdCate { get; set; }
        public string CategoryName { get; set; }
    }
}
