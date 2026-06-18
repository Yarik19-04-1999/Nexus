using Dvizh.Application.Models;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.GetInvites;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetInvitesResponseMapper
{
    public static GetInvitesResponse Map(PagedResult<Invite> result)
        => new(result.Items.Select(MapItem).ToList(), result.TotalCount, result.Page, result.PageSize, result.TotalPages);

    public static partial GetInviteItemResponse MapItem(Invite invite);
}
