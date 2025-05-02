using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Category
{
    public class CategoryUpdateRequest
    {
        [Required]
        public int IdCate { get; set; }

        [Required]
        public string CateName { get; set; }

        public int? ParentId { get; set; }
    }


}
