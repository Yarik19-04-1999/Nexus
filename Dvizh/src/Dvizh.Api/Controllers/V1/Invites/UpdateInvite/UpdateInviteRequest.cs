using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.UpdateInvite;

public record UpdateInviteRequest(int Id, string Message, string? Description, DateTime? ExpiresAt, InviteLanguage Language, InviteMascot Mascot);
