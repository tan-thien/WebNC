using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductUpdateViewModel
    {
        public int IdProduct { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        public int? BasePrice { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }

        public int IdCate { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<ProductImageViewModel>? ExistingImages { get; set; }
    }
}
