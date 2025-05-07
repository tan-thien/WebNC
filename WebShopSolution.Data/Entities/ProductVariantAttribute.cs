using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSolution.Data.Entities
{
    public class ProductVariantAttribute
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProductVariant")]
        public int VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        [Required]
        public string AttributeName { get; set; } // Ví dụ: Color, Size

        [Required]
        public string AttributeValue { get; set; } // Ví dụ: Red, XL
    }
}
