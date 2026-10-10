using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    public static ComponentDTO ConvertFrom(Component e)
    {
        return new ComponentDTO
        {
            Id = e.Id,
            Name = e.Name,
            Content = e.Content,
        };
    }

    public static Component ConvertTo(ComponentDTO d)
    {
        return new Component
        {
            Id = d.Id,
            Name = d.Name,
            Content = d.Content,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(Component e)
    {
        e.Name = Name;
        e.Content = Content;
    }
}
