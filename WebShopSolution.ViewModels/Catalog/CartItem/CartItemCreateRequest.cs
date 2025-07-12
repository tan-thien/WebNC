using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class CartItemCreateRequest
    {

        public int IdCart { get; set; }

        [Required]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
        public int? VariantId { get; set; }

    }
}
