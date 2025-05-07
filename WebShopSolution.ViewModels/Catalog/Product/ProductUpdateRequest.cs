using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductUpdateRequest
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public int? BasePrice { get; set; }
        public int? Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
