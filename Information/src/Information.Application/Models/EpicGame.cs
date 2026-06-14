namespace Information.Application.Models;

public class EpicGame
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public DateTimeOffset FreeUntil { get; init; }
    public string StoreUrl { get; init; } = default!;
}
