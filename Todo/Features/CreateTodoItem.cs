using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Entities;
using Todo.Shared;

namespace Todo.Features;

[ApiController]
[Route("v1")]
public class CreateTodoItemController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodoItem(
        [FromServices] IMediator mediator,
        [FromBody] CreateTodoItemViewModel createTodoItemViewModel)
    {
        TodoItem todoItem = await mediator.Send(
            new CreateTodoItemCommand(
                createTodoItemViewModel.Title,
                createTodoItemViewModel.Description));
        return Created($"v1/todos/{todoItem.Id}", todoItem);
    }
}

public record CreateTodoItemCommand(string Title, string Description) : IRequest<TodoItem>;

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItem>
{
    private readonly DataContext _context;

    public CreateTodoItemCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem(request.Title, request.Description);
        _context.Todos.Add(todoItem);
        await _context.SaveChangesAsync(cancellationToken);
        return todoItem; ;
    }
}

public class CreateTodoItemViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
}