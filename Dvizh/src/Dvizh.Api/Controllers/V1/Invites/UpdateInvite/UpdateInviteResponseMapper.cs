using Dvizh.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.UpdateInvite;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class UpdateInviteResponseMapper
{
    public static partial UpdateInviteResponse ToResponse(this Invite invite);
}
