namespace PAW.Web.Services;

public abstract class ServiceBase
{
    protected string BaseUrl { get; set; } = "https://localhost:7038/";

    protected string SetPathUrl(string name) => $"{BaseUrl}{name}";

    protected static bool ParseBool(string? response) => bool.TryParse(response?.Trim(), out var ok) && ok;

}
