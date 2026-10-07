using PAW.Models.DTO;

namespace PAW.Web.Models
{
    /// <summary>Product + its related inventory, category and supplier (Product details partial view).</summary>
    public class ProductDetailsViewModel
    {
        public ProductDTO Product { get; set; } = new();
        public InventoryDTO? Inventory { get; set; }
        public CategoryDTO? Category { get; set; }
        public SupplierDTO? Supplier { get; set; }
    }

    /// <summary>An item (category, supplier, inventory) with the products associated to it.</summary>
    public class DetailsWithProductsViewModel<T>
    {
        public T Item { get; set; } = default!;
        public List<ProductDTO> Products { get; set; } = [];
    }
}
