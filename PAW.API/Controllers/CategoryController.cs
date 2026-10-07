using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController(ICategoryRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetCategories")]
    public async Task<IEnumerable<CategoryDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(CategoryDTO.ConvertFrom).ToList();
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public async Task<ActionResult<CategoryDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();

        return CategoryDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateCategory")]
    public async Task<IActionResult> Create([FromBody] CategoryDTO dto)
    {
        var created = await repository.CreateAsync(CategoryDTO.ConvertTo(dto));
        if (!created) return BadRequest("No se pudo crear la categoría.");

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:int}", Name = "UpdateCategory")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto)
    {
        if (dto.CategoryId != id)
            return BadRequest("El id de la ruta no coincide con el del cuerpo.");

        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);

        var updated = await repository.UpdateAsync(existing);
        if (!updated) return BadRequest("No se pudo actualizar la categoría.");

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        var deleted = await repository.DeleteAsync(existing);
        if (!deleted) return BadRequest("No se pudo eliminar la categoría.");

        return NoContent();
    }
}