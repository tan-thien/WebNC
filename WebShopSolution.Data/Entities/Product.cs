using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; }


        public string? Image { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Size is required.")]
        public string Size { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "Category is required.")]
        public int IdCate { get; set; }
        public Category Category { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
