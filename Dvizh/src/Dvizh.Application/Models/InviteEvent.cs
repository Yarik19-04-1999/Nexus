using Dvizh.Application.Enums;
using Nexus.Application.Core.Interfaces;

namespace Dvizh.Application.Models;

public class InviteEvent : IHasCreatedAt, IHasLongPrimaryIdentifier
{
    public DateTime CreatedAt { get; set; }
    public long Id { get; set; }
    public int InviteId { get; set; }
    public InviteEventType EventType { get; set; }

    public Invite Invite { get; set; } = null!;
}
