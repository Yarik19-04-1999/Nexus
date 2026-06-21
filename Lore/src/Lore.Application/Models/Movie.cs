using Lore.Application.Models.Enums;
using Nexus.Application.Core.Interfaces;

namespace Lore.Application.Models;

public class Movie : BaseEntity
{
    public int? UniverseId { get; set; }
    public Universe? Universe { get; set; }
    public string Title { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public int DurationMinutes { get; set; }
    public string? ReviewText { get; set; }
    public decimal? Score { get; set; }
    public int ViewCount { get; set; } = 1;
    public RewatchStatus RewatchStatus { get; set; }
    public int ListNo { get; set; }
}
