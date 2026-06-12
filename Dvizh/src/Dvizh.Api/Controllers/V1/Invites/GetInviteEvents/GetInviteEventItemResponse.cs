using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.GetInviteEvents;

public record GetInviteEventItemResponse(
    long Id,
    int InviteId,
    InviteEventType EventType,
    DateTime CreatedAt
);
