using Nexus.Application.Core.Interfaces;
using Sieve.Attributes;

namespace Lore.Application.Models;

public class Universe : BaseEntity, IHasIsHidden
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Sieve(CanFilter = true)]
    public bool IsHidden { get; set; }

    [Sieve(CanSort = true)]
    public int ListNo { get; set; }
}
