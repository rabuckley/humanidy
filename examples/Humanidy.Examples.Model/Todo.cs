namespace Humanidy.Examples.Model;

public sealed record Todo
{
    public required TodoId Id { get; init; }

    public required string Title { get; init; }

    public DateOnly? DueBy { get; init; }

    public bool IsComplete { get; init; }
}
