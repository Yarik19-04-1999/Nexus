using Nexus.Application.Core.Interfaces;

namespace Lore.Application.Models;

public class BaseEntity : IHasCreatedAt, IHasUpdatedAt, IHasPrimaryIdentifier
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Id { get; set; }
}
