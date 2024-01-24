using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Data;

public class DataContext : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Test");
    }
}