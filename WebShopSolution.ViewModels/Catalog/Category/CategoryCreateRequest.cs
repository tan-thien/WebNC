using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Category
{
    public class CategoryCreateRequest
    {
        [Required]
        public string CateName { get; set; }
    }
}
