using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetSuppliers")]
    public async Task<IEnumerable<SupplierDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(SupplierDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetSupplierById")]
    public async Task<ActionResult<SupplierDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return SupplierDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateSupplier")]
    public async Task<ActionResult<bool>> Create([FromBody] SupplierDTO dto)
    {
        return await repository.CreateAsync(SupplierDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateSupplier")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] SupplierDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteSupplier")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
