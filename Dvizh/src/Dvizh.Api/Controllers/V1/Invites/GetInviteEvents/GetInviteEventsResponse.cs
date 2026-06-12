namespace Dvizh.Api.Controllers.V1.Invites.GetInviteEvents;

public record GetInviteEventsResponse(
    IReadOnlyList<GetInviteEventItemResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
