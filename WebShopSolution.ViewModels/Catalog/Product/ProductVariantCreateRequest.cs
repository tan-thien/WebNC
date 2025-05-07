
namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductVariantCreateRequest
    {
        public int ProductId { get; set; }       // Id sản phẩm chính
        public string Sku { get; set; }          // Mã SKU riêng cho biến thể
        public int Price { get; set; }
        public int Stock { get; set; }

        // Nếu có thêm thuộc tính cho biến thể (màu sắc, size...) thì thêm:
        public List<ProductVariantAttributeCreateRequest> Attributes { get; set; } = new();
    }
}
