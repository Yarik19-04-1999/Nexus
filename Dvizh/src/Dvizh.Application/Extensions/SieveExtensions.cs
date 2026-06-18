using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;
using Sieve.Models;
using Sieve.Services;

namespace Dvizh.Application.Extensions;

public static class SieveExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this ISieveProcessor sieve,
        SieveModel sieveModel,
        IQueryable<T> query,
        CancellationToken cancellationToken = default)
    {
        var total = await sieve.Apply(sieveModel, query, applyPagination: false).CountAsync(cancellationToken);
        var items = await sieve.Apply(sieveModel, query).ToListAsync(cancellationToken);
        return new PagedResult<T>(
            items,
            total,
            sieveModel.Page ?? PagingConstants.DefaultPage,
            sieveModel.PageSize ?? PagingConstants.DefaultPageSize);
    }
}
