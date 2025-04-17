using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Category
    {
        [Key]
        public int IdCate { get; set; }


        public string CateName { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
