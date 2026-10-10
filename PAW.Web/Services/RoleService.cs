using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetAllAsync();
    Task<RoleDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(RoleDTO dto);
    Task<bool> UpdateAsync(int id, RoleDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<RoleDTO>> GetAllAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        return await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);
    }

    public async Task<RoleDTO?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<RoleDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(RoleDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return ParseBool(response);
    }

    public async Task<bool> UpdateAsync(int id, RoleDTO dto)
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
