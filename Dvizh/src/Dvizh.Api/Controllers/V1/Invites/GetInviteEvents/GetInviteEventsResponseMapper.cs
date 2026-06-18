using Dvizh.Application.Models;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.GetInviteEvents;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetInviteEventsResponseMapper
{
    public static GetInviteEventsResponse Map(PagedResult<InviteEvent> result)
        => new(result.Items.Select(MapItem).ToList(), result.TotalCount, result.Page, result.PageSize, result.TotalPages);

    public static partial GetInviteEventItemResponse MapItem(InviteEvent inviteEvent);
}
