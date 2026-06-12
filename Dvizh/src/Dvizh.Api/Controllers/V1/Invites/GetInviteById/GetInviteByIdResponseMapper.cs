using Dvizh.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.GetInviteById;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetInviteByIdResponseMapper
{
    public static partial GetInviteByIdResponse ToResponse(this Invite invite);
}
