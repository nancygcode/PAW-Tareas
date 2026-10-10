using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController(ILogger<RoleController> logger, IRoleRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetRoles")]
    public async Task<IEnumerable<RoleDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(RoleDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetRoleById")]
    public async Task<ActionResult<RoleDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return RoleDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateRole")]
    public async Task<ActionResult<bool>> Create([FromBody] RoleDTO dto)
    {
        return await repository.CreateAsync(RoleDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateRole")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] RoleDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteRole")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
