namespace Dvizh.Api.Controllers.V1.Invites.UpdateInvite;

public record UpdateInviteRequest(string Message, string? Description, DateTime? ExpiresAt);
