using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSolution.Data.Entities
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to Product
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string Sku { get; set; }

        public int Price { get; set; }
        public int Stock { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<ProductVariantAttribute> Attributes { get; set; }
    }
}
