namespace Dvizh.Api.Controllers.V1.Invites.GetInvites;

public record GetInvitesResponse(
    IReadOnlyList<GetInviteItemResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
