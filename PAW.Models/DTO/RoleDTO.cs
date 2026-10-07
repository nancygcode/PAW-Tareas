using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class RoleDTO
{
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
    [JsonPropertyName("roleName")]
    public string? RoleName { get; set; }

    public static RoleDTO ConvertFrom(Role e)
    {
        return new RoleDTO
        {
            RoleId = e.RoleId,
            RoleName = e.RoleName,
        };
    }

    public static Role ConvertTo(RoleDTO d)
    {
        return new Role
        {
            RoleId = d.RoleId,
            RoleName = d.RoleName,
        };
    }

    /// <summary>Copies the editable values onto an entity that already exists in the database.</summary>
    public void ApplyTo(Role e)
    {
        e.RoleName = RoleName;
    }
}
