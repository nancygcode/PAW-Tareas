using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class InventoryDTO
{
    [JsonPropertyName("inventoryId")]
    public int InventoryId { get; set; }
    [JsonPropertyName("unitPrice")]
    public decimal? UnitPrice { get; set; }
    [JsonPropertyName("unitsInStock")]
    public int? UnitsInStock { get; set; }
    [JsonPropertyName("lastUpdated")]
    public DateTime? LastUpdated { get; set; }
    [JsonPropertyName("productId")]
    public int? ProductId { get; set; }
    [JsonPropertyName("dateAdded")]
    public DateTime? DateAdded { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static InventoryDTO ConvertFrom(Inventory e)
    {
        return new InventoryDTO
        {
            InventoryId = e.InventoryId,
            UnitPrice = e.UnitPrice,
            UnitsInStock = e.UnitsInStock,
            LastUpdated = e.LastUpdated,
            ProductId = e.ProductId,
            DateAdded = e.DateAdded,
            ModifiedBy = e.ModifiedBy,
        };
    }

    public static Inventory ConvertTo(InventoryDTO d)
    {
        return new Inventory
        {
            InventoryId = d.InventoryId,
            UnitPrice = d.UnitPrice,
            UnitsInStock = d.UnitsInStock,
            LastUpdated = DateTime.Now,
            ProductId = d.ProductId,
            DateAdded = d.DateAdded ?? DateTime.Now,
            ModifiedBy = d.ModifiedBy,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(Inventory e)
    {
        e.UnitPrice = UnitPrice;
        e.UnitsInStock = UnitsInStock;
        e.LastUpdated = DateTime.Now;
        e.ProductId = ProductId;
        if (!string.IsNullOrWhiteSpace(ModifiedBy)) e.ModifiedBy = ModifiedBy;
    }
}
