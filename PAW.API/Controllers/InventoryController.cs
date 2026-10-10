using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController(ILogger<InventoryController> logger, IInventoryRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetInventories")]
    public async Task<IEnumerable<InventoryDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(InventoryDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetInventoryById")]
    public async Task<ActionResult<InventoryDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return InventoryDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateInventory")]
    public async Task<ActionResult<bool>> Create([FromBody] InventoryDTO dto)
    {
        return await repository.CreateAsync(InventoryDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateInventory")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] InventoryDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteInventory")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
