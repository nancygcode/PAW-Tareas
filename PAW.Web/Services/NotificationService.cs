using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetAllAsync();
    Task<NotificationDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(NotificationDTO dto);
    Task<bool> UpdateAsync(int id, NotificationDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<NotificationDTO>> GetAllAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        return await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
    }

    public async Task<NotificationDTO?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(NotificationDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return ParseBool(response);
    }

    public async Task<bool> UpdateAsync(int id, NotificationDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl(_path), id.ToString(), JsonProvider.Serialize(dto));
        return ParseBool(response);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return ParseBool(response);
    }
}
