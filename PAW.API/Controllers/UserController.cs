using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(ILogger<UserController> logger, IUserRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetUsers")]
    public async Task<IEnumerable<UserDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(UserDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetUserById")]
    public async Task<ActionResult<UserDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return UserDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<ActionResult<bool>> Create([FromBody] UserDTO dto)
    {
        return await repository.CreateAsync(UserDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateUser")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] UserDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteUser")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
