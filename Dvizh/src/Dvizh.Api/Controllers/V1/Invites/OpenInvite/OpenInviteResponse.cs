using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.OpenInvite;

public record OpenInviteResponse(
    int Id,
    string Code,
    string Message,
    string? Description,
    DateTime? ExpiresAt,
    InviteAnswer Answer,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
