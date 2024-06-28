using ApiWithDapper.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithDapper.Todo;

[ApiController]
[Route("[controller]")]
public class TodoController(ITodoRepository todoRepo) : ControllerBase {
    [HttpGet("All")]
    public async Task<ActionResult<PageData<Todo>>> GetAll(bool? completed, string? contains, int? limit, int? page) {
        limit ??= 10;
        page ??= 1;
        var todos = await todoRepo.GetAllAsync(completed, contains, limit, page);
        return Ok(todos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Todo>> GetById(int id) {
        var item = await todoRepo.GetByIdAsync(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost("Create")]
    public async Task<ActionResult> Create(Todo input) {
        var entityId = await todoRepo.CreateAsync(input);
        input.Id = entityId;
        return CreatedAtAction("GetById", new { id = entityId }, input);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, Todo input) {
        if (id != input.Id) return BadRequest();
        var affected = await todoRepo.UpdateAsync(input);
        if (affected < 1) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id) {
        var affected = await todoRepo.DeleteAsync(id);
        if (affected < 1) return NotFound();
        return NoContent();
    }

    [HttpDelete("All")]
    public async Task<ActionResult> DeleteAll() {
        await todoRepo.DeleteAllAsync();
        return NoContent();
    }
}