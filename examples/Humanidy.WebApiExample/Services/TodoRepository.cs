using Humanidy.Examples.Model;

namespace Humanidy.WebApiExample.Services;

public sealed class TodoRepository
{
    private readonly List<Todo> _todos =
    [
        new()
        {
            Id = TodoId.Parse("todo_8u1AtGwvGEfhI"),
            Title = "Buy groceries",
        },
        new()
        {
            Id = TodoId.NewId(),
            Title = "Walk the dog",
            DueBy = new DateOnly(2023, 10, 15),
        },
        new()
        {
            Id = TodoId.NewId(),
            Title = "Finish project report",
            IsComplete = true,
        }
    ];

    public IEnumerable<Todo> List()
    {
        return _todos;
    }

    public Todo? GetById(TodoId id)
    {
        return _todos.FirstOrDefault(todo => todo.Id == id);
    }
}
