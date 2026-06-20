using Dvizh.Api.Controllers.V1.Invites.GetInviteEvents.Dtos;
using Dvizh.Application.Models;
using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Dvizh.Api.Controllers.V1.Invites.GetInviteEvents;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetInviteEventsResponseMapper
{
    public static PagedResponse<GetInviteEventDto> Map(PagedResult<InviteEvent> result)
        => result.ToPagedResponse(MapItem);

    public static partial GetInviteEventDto MapItem(InviteEvent inviteEvent);
}
