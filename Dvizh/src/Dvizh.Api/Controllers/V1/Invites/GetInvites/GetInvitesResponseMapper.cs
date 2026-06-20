using Dvizh.Api.Controllers.V1.Invites.GetInvites.Dtos;
using Dvizh.Application.Models;
using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.GetInvites;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetInvitesResponseMapper
{
    public static PagedResponse<GetInviteDto> Map(PagedResult<Invite> result)
        => result.ToPagedResponse(MapItem);

    public static partial GetInviteDto MapItem(Invite invite);
}
