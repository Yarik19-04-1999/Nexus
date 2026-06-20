using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Models;

namespace Nexus.Api.Core.Extensions;

public static class PagedResultExtensions
{
    public static PagedResponse<TResponse> ToPagedResponse<TResult, TResponse>(
        this PagedResult<TResult> result,
        Func<TResult, TResponse> mapItem)
        => new(
            result.Items.Select(mapItem).ToList(),
            result.TotalCount,
            result.Page,
            result.PageSize,
            result.TotalPages);
}
