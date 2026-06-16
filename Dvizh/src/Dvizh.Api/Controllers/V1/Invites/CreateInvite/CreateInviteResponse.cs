using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.CreateInvite;

public record CreateInviteResponse(
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
