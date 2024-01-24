using Microsoft.EntityFrameworkCore;
using Todo.Entities;

namespace Todo.Shared;

public class DataContext : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Test");
    }
}