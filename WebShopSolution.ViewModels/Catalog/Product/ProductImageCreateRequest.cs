using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductImageCreateRequest
    {
        public int ProductId { get; set; }
        public string ImagePath { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
