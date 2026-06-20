using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.GetInviteEvents.Dtos;

public record GetInviteEventDto(
    long Id,
    int InviteId,
    InviteEventType EventType,
    DateTime CreatedAt
);
