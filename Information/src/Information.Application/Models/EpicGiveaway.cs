namespace Information.Application.Models;

public class EpicGiveaway
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Url { get; init; } = default!;
    public string? ThumbnailUrl { get; init; }
    public DateTime? AvailableUntil { get; init; }
}
