using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Shared;

namespace Todo.Features;

[ApiController]
[Route("v1")]
public class StartTodoItemController : ControllerBase
{
    [HttpPut("{id}/start")]
    public async Task<IActionResult> StartTodo(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id)
    {
        bool commandResult = await mediator.Send(new StartTodoItemCommand(id));
        if (!commandResult)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public record StartTodoItemCommand(Guid Id) : IRequest<bool>;

public class StartTodoItemCommandHandler : IRequestHandler<StartTodoItemCommand, bool>
{
    private readonly DataContext _context;

    public StartTodoItemCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(StartTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.Todos.FindAsync(request.Id);
        if (todoItem == null)
        {
            return false;
        }

        todoItem.StartTodo();
        return await _context.SaveChangesAsync(cancellationToken) == 1;
    }
}