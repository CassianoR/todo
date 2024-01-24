using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Shared;

namespace Todo.Features;

[ApiController]
[Route("v1")]
public class DeleteTodoItemController : ControllerBase
{
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id)
    {
        bool commandResult = await mediator.Send(new DeleteTodoItemCommand(id));
        if (!commandResult)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public record DeleteTodoItemCommand(Guid Id) : IRequest<bool>;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, bool>
{
    private readonly DataContext _context;

    public DeleteTodoItemCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.Todos.FindAsync(request.Id);
        if (todoItem == null)
        {
            return false;
        }

        _context.Todos.Remove(todoItem);
        return await _context.SaveChangesAsync(cancellationToken) == 1;
    }
}