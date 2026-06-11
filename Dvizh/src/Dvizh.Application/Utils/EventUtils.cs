using Dvizh.Application.Enums;
using Dvizh.Application.Models;

namespace Dvizh.Application.Utils;

public static class EventUtils
{
    public static InviteEvent CreateEvent(Invite invite, InviteEventType eventType)
        => CreateEvent(invite.Id, eventType);

    public static InviteEvent CreateEvent(int id, InviteEventType eventType)
        => new()
        {
            InviteId = id,
            EventType = eventType
        };
}
