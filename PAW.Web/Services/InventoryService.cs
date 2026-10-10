using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetAllAsync();
    Task<InventoryDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(InventoryDTO dto);
    Task<bool> UpdateAsync(int id, InventoryDTO dto);
    Task<bool> DeleteAsync(int id);
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<InventoryDTO>> GetAllAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        return await JsonProvider.DeserializeAsync<IEnumerable<InventoryDTO>>(response);
    }

    public async Task<InventoryDTO?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(InventoryDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return ParseBool(response);
    }

    public async Task<bool> UpdateAsync(int id, InventoryDTO dto)
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
