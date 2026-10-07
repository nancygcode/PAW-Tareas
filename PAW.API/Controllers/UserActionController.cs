using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserActionController(ILogger<UserActionController> logger, IUserActionRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetUserActions")]
    public async Task<IEnumerable<UserActionDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(UserActionDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetUserActionById")]
    public async Task<ActionResult<UserActionDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return UserActionDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateUserAction")]
    public async Task<ActionResult<bool>> Create([FromBody] UserActionDTO dto)
    {
        return await repository.CreateAsync(UserActionDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateUserAction")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] UserActionDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteUserAction")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
