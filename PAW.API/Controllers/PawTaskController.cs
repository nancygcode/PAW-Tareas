using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PawTaskController(ILogger<PawTaskController> logger, IPawTaskRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetPawTasks")]
    public async Task<IEnumerable<PawTaskDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(PawTaskDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetPawTaskById")]
    public async Task<ActionResult<PawTaskDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return PawTaskDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreatePawTask")]
    public async Task<ActionResult<bool>> Create([FromBody] PawTaskDTO dto)
    {
        return await repository.CreateAsync(PawTaskDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdatePawTask")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] PawTaskDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeletePawTask")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
