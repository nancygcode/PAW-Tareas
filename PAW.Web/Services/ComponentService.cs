using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<IEnumerable<ComponentDTO>> GetAllAsync();
    Task<ComponentDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(ComponentDTO dto);
    Task<bool> UpdateAsync(int id, ComponentDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ComponentDTO>> GetAllAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        return await JsonProvider.DeserializeAsync<IEnumerable<ComponentDTO>>(response);
    }

    public async Task<ComponentDTO?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(ComponentDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return ParseBool(response);
    }

    public async Task<bool> UpdateAsync(int id, ComponentDTO dto)
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
