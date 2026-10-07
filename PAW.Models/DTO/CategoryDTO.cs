using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }
    [JsonPropertyName("categoryName")]
    public string? CategoryName { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static CategoryDTO ConvertFrom(Category e)
    {
        return new CategoryDTO
        {
            CategoryId = e.CategoryId,
            CategoryName = e.CategoryName,
            Description = e.Description,
            LastModified = e.LastModified,
            ModifiedBy = e.ModifiedBy,
        };
    }

    public static Category ConvertTo(CategoryDTO d)
    {
        return new Category
        {
            CategoryId = d.CategoryId,
            CategoryName = d.CategoryName,
            Description = d.Description,
            LastModified = DateTime.Now,
            ModifiedBy = d.ModifiedBy,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(Category e)
    {
        e.CategoryName = CategoryName;
        e.Description = Description;
        e.LastModified = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(ModifiedBy)) e.ModifiedBy = ModifiedBy;
    }
}
