using Dvizh.Application.Enums;

namespace Dvizh.Api.Controllers.V1.Invites.RespondToInvite;

public record RespondToInviteRequest(string Code, InviteAnswer Answer);
