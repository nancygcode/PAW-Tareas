using System.Text.Json.Serialization;
using PawTask = PAW.Models.Task;

namespace PAW.Models.DTO;

public class PawTaskDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static PawTaskDTO ConvertFrom(PawTask e)
    {
        return new PawTaskDTO
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Status = e.Status,
            DueDate = e.DueDate,
            CreatedAt = e.CreatedAt,
            LastModified = e.LastModified,
            ModifiedBy = e.ModifiedBy,
        };
    }

    public static PawTask ConvertTo(PawTaskDTO d)
    {
        return new PawTask
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            Status = d.Status,
            DueDate = d.DueDate,
            CreatedAt = d.CreatedAt ?? DateTime.Now,
            LastModified = DateTime.Now,
            ModifiedBy = d.ModifiedBy,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(PawTask e)
    {
        e.Name = Name;
        e.Description = Description;
        e.Status = Status;
        e.DueDate = DueDate;
        e.LastModified = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(ModifiedBy)) e.ModifiedBy = ModifiedBy;
    }
}
