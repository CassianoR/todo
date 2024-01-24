using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Entities;
using Todo.Shared;

namespace Todo.Features;

[ApiController]
[Route("v1")]
public class GetTodoItemByIdController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id)
    {
        var todoItem = await mediator.Send(new GetTodoItemByIdQuery(id));
        if (todoItem == null)
        {
            return NotFound();
        }

        return Ok(todoItem);
    }
}

public record GetTodoItemByIdQuery(Guid Id) : IRequest<TodoItem?>;

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItem?>
{
    private readonly DataContext _context;

    public GetTodoItemByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<TodoItem?> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Todos.FindAsync(request.Id);
    }
}