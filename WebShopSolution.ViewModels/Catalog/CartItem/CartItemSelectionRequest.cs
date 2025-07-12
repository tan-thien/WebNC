using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.CartItem
{
    public class CartItemSelectionRequest
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; } // Có thể null nếu không có biến thể
        public int Quantity { get; set; }
        public int Price { get; set; } // Giá * số lượng => dùng để tính TotalAmount
        public string? ProductName { get; set; }
        public string? ImagePath { get; set; }
        public string? VariantInfo { get; set; }

    }
}
