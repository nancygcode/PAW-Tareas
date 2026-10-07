using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController(ILogger<ComponentController> logger, IComponentRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetComponents")]
    public async Task<IEnumerable<ComponentDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(ComponentDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetComponentById")]
    public async Task<ActionResult<ComponentDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return ComponentDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateComponent")]
    public async Task<ActionResult<bool>> Create([FromBody] ComponentDTO dto)
    {
        return await repository.CreateAsync(ComponentDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateComponent")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] ComponentDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteComponent")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
