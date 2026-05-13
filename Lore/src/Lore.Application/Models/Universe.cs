using Nexus.Application.Core.Interfaces;

namespace Lore.Application.Models;

public class Universe : BaseEntity, IHasIsHidden
{
    public bool IsHidden { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ListNo { get; set; }
}
