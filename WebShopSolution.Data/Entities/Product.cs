using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSolution.Data.Entities
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        public string? Description { get; set; }


        public int? BasePrice { get; set; }


        public int? Quantity { get; set; }

        public string? Status { get; set; }

        // Foreign key to Category
        [ForeignKey("Category")]
        [Required]
        public int IdCate { get; set; }
        public Category Category { get; set; }

        // Navigation properties
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
