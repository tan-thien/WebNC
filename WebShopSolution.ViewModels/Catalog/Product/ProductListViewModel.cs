using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Category;

namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductListViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
