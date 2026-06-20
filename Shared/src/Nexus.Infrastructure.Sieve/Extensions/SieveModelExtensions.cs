using Nexus.Application.Core.Constants;
using Sieve.Models;

namespace Nexus.Infrastructure.Sieve.Extensions;

public static class SieveModelExtensions
{
    public static int GetPageNumberOrDefault(this SieveModel model)
        => model.Page ?? PagingConstants.DefaultPage;

    public static int GetPageSizeOrDefault(this SieveModel model)
        => model.PageSize ?? PagingConstants.DefaultPageSize;
}
