using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }
    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("lastModifiedBy")]
    public string? LastModifiedBy { get; set; }

    public static UserDTO ConvertFrom(User e)
    {
        return new UserDTO
        {
            UserId = e.UserId,
            Username = e.Username,
            Email = e.Email,
            IsActive = e.IsActive,
            RoleId = e.RoleId,
            CreatedAt = e.CreatedAt,
            LastModified = e.LastModified,
            ModifiedBy = e.ModifiedBy,
            LastModifiedBy = e.LastModifiedBy,
        };
    }

    public static User ConvertTo(UserDTO d)
    {
        return new User
        {
            UserId = d.UserId,
            Username = d.Username,
            Email = d.Email,
            IsActive = d.IsActive,
            RoleId = d.RoleId,
            CreatedAt = d.CreatedAt ?? DateTime.Now,
            LastModified = DateTime.Now,
            ModifiedBy = d.ModifiedBy,
            LastModifiedBy = d.LastModifiedBy,
            PasswordHash = string.IsNullOrWhiteSpace(d.Password) ? null : HashPassword(d.Password),
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(User e)
    {
        e.Username = Username;
        e.Email = Email;
        e.IsActive = IsActive;
        e.RoleId = RoleId;
        e.LastModified = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(ModifiedBy)) e.ModifiedBy = ModifiedBy;
        if (!string.IsNullOrWhiteSpace(LastModifiedBy)) e.LastModifiedBy = LastModifiedBy;
        if (!string.IsNullOrWhiteSpace(Password)) e.PasswordHash = HashPassword(Password);
    }

    /// <summary>SHA-256 (hex) of the plain password. The plain value is never stored or returned by the API.</summary>
    public static string HashPassword(string password) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
}
