using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class SupplierDTO
{
    [JsonPropertyName("supplierId")]
    public int SupplierId { get; set; }
    [JsonPropertyName("supplierName")]
    public string? SupplierName { get; set; }
    [JsonPropertyName("contactName")]
    public string? ContactName { get; set; }
    [JsonPropertyName("contactTitle")]
    public string? ContactTitle { get; set; }
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [JsonPropertyName("city")]
    public string? City { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static SupplierDTO ConvertFrom(Supplier e)
    {
        return new SupplierDTO
        {
            SupplierId = e.SupplierId,
            SupplierName = e.SupplierName,
            ContactName = e.ContactName,
            ContactTitle = e.ContactTitle,
            Phone = e.Phone,
            Address = e.Address,
            City = e.City,
            Country = e.Country,
            LastModified = e.LastModified,
            ModifiedBy = e.ModifiedBy,
        };
    }

    public static Supplier ConvertTo(SupplierDTO d)
    {
        return new Supplier
        {
            SupplierId = d.SupplierId,
            SupplierName = d.SupplierName,
            ContactName = d.ContactName,
            ContactTitle = d.ContactTitle,
            Phone = d.Phone,
            Address = d.Address,
            City = d.City,
            Country = d.Country,
            LastModified = DateTime.Now,
            ModifiedBy = d.ModifiedBy,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(Supplier e)
    {
        e.SupplierName = SupplierName;
        e.ContactName = ContactName;
        e.ContactTitle = ContactTitle;
        e.Phone = Phone;
        e.Address = Address;
        e.City = City;
        e.Country = Country;
        e.LastModified = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(ModifiedBy)) e.ModifiedBy = ModifiedBy;
    }
}
