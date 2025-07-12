using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class CartItemViewModel
    {
        public int IdCart { get; set; }
        public int ProductId { get; set; }

        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Attributes { get; set; } // "Size: M, Color: Đỏ"

        public string ImagePath { get; set; }
        public int Price { get; set; }
        public int? VariantId { get; set; } // ✅ Thêm VariantId
        public string? VariantSummary { get; set; } // <--- THÊM DÒNG NÀY

    }
}
