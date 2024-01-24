using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Controllers;

[ApiController]
[Route("v1/[controller]/[action]")]
public class TodoController : ControllerBase
{
    private readonly DataContext _context;

    public TodoController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
    {
        var todos = await _context.Todos.ToListAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(Guid id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        return Ok(todoItem);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodoItem([FromBody] CreateTodoItemViewModel createTodoItemViewModel)
    {
        var todoItem = new TodoItem(createTodoItemViewModel.Title, createTodoItemViewModel.Description);
        _context.Todos.Add(todoItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}/start")]
    public async Task<IActionResult> StartTodo(Guid id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.StartTodo();
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/finalize")]
    public async Task<IActionResult> FinalizeTodo(Guid id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.FinalizeTodo();
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.Todos.Remove(todoItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}