using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Shared;

namespace Todo.Features;

[ApiController]
[Route("v1")]
public class FinalizeTodoItemController : ControllerBase
{
    [HttpPut("{id}/finalize")]
    public async Task<IActionResult> FinalizeTodo(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id)
    {
        bool commandResult = await mediator.Send(new FinalizeTodoItemCommand(id));
        if (!commandResult)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public record FinalizeTodoItemCommand(Guid Id) : IRequest<bool>;

public class FinalizeTodoItemCommandHandler : IRequestHandler<FinalizeTodoItemCommand, bool>
{
    private readonly DataContext _context;

    public FinalizeTodoItemCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(FinalizeTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.Todos.FindAsync(request.Id);
        if (todoItem == null)
        {
            return false;
        }

        todoItem.FinalizeTodo();
        return await _context.SaveChangesAsync(cancellationToken) == 1;
    }
}