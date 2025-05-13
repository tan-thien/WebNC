using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductCreateViewModel
    {
        [Required]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Create as variant product")]
        public bool IsVariantProduct { get; set; }

        // Chỉ áp dụng khi không phải sản phẩm biến thể
        public int? BasePrice { get; set; }

        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required]
        public int IdCate { get; set; }

        public List<IFormFile>? Images { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }

}
