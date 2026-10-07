using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserRoleDTO
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }
    [JsonPropertyName("roldId")]
    public decimal? RoldId { get; set; }
    [JsonPropertyName("userId")]
    public decimal? UserId { get; set; }

    public static UserRoleDTO ConvertFrom(UserRole e)
    {
        return new UserRoleDTO
        {
            Id = e.Id,
            RoldId = e.RoldId,
            UserId = e.UserId,
        };
    }

    public static UserRole ConvertTo(UserRoleDTO d)
    {
        return new UserRole
        {
            Id = d.Id,
            RoldId = d.RoldId,
            UserId = d.UserId,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(UserRole e)
    {
        e.RoldId = RoldId;
        e.UserId = UserId;
    }
}
