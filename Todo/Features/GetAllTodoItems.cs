using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Entities;
using Todo.Shared;

namespace Todo.Features;

[ApiController]
[Route("v1")]
public class GetAllTodoItemsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> Get([FromServices] IMediator mediator)
    {
        var todos = await mediator.Send(new GetAllTodoItemsQuery());
        return Ok(todos);
    }
}

public record GetAllTodoItemsQuery : IRequest<List<TodoItem>>;

public class GetAllTodoItemsQueryHandler : IRequestHandler<GetAllTodoItemsQuery, List<TodoItem>>
{
    private readonly DataContext _context;

    public GetAllTodoItemsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> Handle(GetAllTodoItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Todos.ToListAsync(cancellationToken);
    }
}