using System.ComponentModel.DataAnnotations;

namespace Todo.Entities;

public class TodoItem
{
    public DateTime CreatedAt { get; protected set; }
    public string Description { get; protected set; }

    [Key]
    public Guid Id { get; protected set; }

    public DateTime UpdatedAt { get; protected set; }
    public TodoItemStatus Status { get; protected set; }
    public string Title { get; protected set; }

    public TodoItem()
    {
    }

    public TodoItem(string title, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void FinalizeTodo()
    {
        Status = TodoItemStatus.Done;
        UpdatedAt = DateTime.Now;
    }

    public void StartTodo()
    {
        Status = TodoItemStatus.InProgress;
        UpdatedAt = DateTime.Now;
    }
}