using Dvizh.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.CreateInvite;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CreateInviteResponseMapper
{
    public static partial CreateInviteResponse ToResponse(this Invite invite);
}
