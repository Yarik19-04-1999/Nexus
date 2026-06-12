using Dvizh.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.OpenInvite;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class OpenInviteResponseMapper
{
    public static partial OpenInviteResponse ToResponse(this Invite invite);
}
