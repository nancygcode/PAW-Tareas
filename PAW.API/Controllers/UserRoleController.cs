using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserRoleController(ILogger<UserRoleController> logger, IUserRoleRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetUserRoles")]
    public async Task<IEnumerable<UserRoleDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(UserRoleDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetUserRoleById")]
    public async Task<ActionResult<UserRoleDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return UserRoleDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateUserRole")]
    public async Task<ActionResult<bool>> Create([FromBody] UserRoleDTO dto)
    {
        return await repository.CreateAsync(UserRoleDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateUserRole")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] UserRoleDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteUserRole")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
