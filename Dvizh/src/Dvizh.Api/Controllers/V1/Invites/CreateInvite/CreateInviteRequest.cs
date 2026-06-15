using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.CreateInvite;

public record CreateInviteRequest(string Message, string? Description, DateTime? ExpiresAt, InviteLanguage Language);
