using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService
{
    Task<IEnumerable<PawTaskDTO>> GetAllAsync();
    Task<PawTaskDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(PawTaskDTO dto);
    Task<bool> UpdateAsync(int id, PawTaskDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class PawTaskService : ServiceBase, IPawTaskService
{
    private const string _path = "PawTask";
    private readonly IRestProvider _restProvider;

    public PawTaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<PawTaskDTO>> GetAllAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        return await JsonProvider.DeserializeAsync<IEnumerable<PawTaskDTO>>(response);
    }

    public async Task<PawTaskDTO?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(PawTaskDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return ParseBool(response);
    }

    public async Task<bool> UpdateAsync(int id, PawTaskDTO dto)
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
