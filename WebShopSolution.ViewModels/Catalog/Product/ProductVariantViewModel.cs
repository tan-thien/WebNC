using WebShopSolution.ViewModels.Catalog.ProductVariantAttributes;


namespace WebShopSolution.ViewModels.Catalog.Product
{
    public class ProductVariantViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Status { get; set; }
        public List<ProductVariantAttributeViewModel> Attributes { get; set; } = new();

    }
}