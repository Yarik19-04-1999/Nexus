using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.GetInvites;

public record GetInviteItemResponse(
    int Id,
    string Code,
    string Message,
    string? Description,
    DateTime? ExpiresAt,
    InviteAnswer Answer,
    InviteLanguage Language,
    InviteMascot Mascot,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
