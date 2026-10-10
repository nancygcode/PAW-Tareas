using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    public static UserActionDTO ConvertFrom(UserAction e)
    {
        return new UserActionDTO
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
        };
    }

    public static UserAction ConvertTo(UserActionDTO d)
    {
        return new UserAction
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(UserAction e)
    {
        e.Name = Name;
        e.Description = Description;
    }
}
