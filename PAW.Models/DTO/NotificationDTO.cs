using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    [JsonPropertyName("isRead")]
    public bool? IsRead { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    public static NotificationDTO ConvertFrom(Notification e)
    {
        return new NotificationDTO
        {
            Id = e.Id,
            UserId = e.UserId,
            Message = e.Message,
            IsRead = e.IsRead,
            CreatedAt = e.CreatedAt,
        };
    }

    public static Notification ConvertTo(NotificationDTO d)
    {
        return new Notification
        {
            Id = d.Id,
            UserId = d.UserId,
            Message = d.Message,
            IsRead = d.IsRead,
            CreatedAt = d.CreatedAt ?? DateTime.Now,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(Notification e)
    {
        e.UserId = UserId;
        e.Message = Message;
        e.IsRead = IsRead;
    }
}
