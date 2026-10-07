using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController(ILogger<NotificationController> logger, INotificationRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetNotifications")]
    public async Task<IEnumerable<NotificationDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(NotificationDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetNotificationById")]
    public async Task<ActionResult<NotificationDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return NotificationDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateNotification")]
    public async Task<ActionResult<bool>> Create([FromBody] NotificationDTO dto)
    {
        return await repository.CreateAsync(NotificationDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateNotification")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] NotificationDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteNotification")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
