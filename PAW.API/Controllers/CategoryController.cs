using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository repository) : ControllerBase
{
    [HttpGet(Name = "GetCategories")]
    public async Task<IEnumerable<CategoryDTO>> GetAll()
    {
        var items = await repository.ReadAsync() ?? [];
        return items.Select(CategoryDTO.ConvertFrom);
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public async Task<ActionResult<CategoryDTO>> GetById(int id)
    {
        var item = await repository.FindAsync(id);
        if (item is null) return NotFound();
        return CategoryDTO.ConvertFrom(item);
    }

    [HttpPost(Name = "CreateCategory")]
    public async Task<ActionResult<bool>> Create([FromBody] CategoryDTO dto)
    {
        return await repository.CreateAsync(CategoryDTO.ConvertTo(dto));
    }

    [HttpPut("{id:int}", Name = "UpdateCategory")]
    public async Task<ActionResult<bool>> Update(int id, [FromBody] CategoryDTO dto)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        dto.ApplyTo(existing);
        return await repository.UpdateAsync(existing);
    }

    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var existing = await repository.FindAsync(id);
        if (existing is null) return NotFound();

        return await repository.DeleteAsync(existing);
    }
}
