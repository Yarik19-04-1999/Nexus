using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Extensions;

public static class HealthChecksBuilderExtensions
{
    public static IHealthChecksBuilder AddNexusSqlServerHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        => builder.AddSqlServer(connectionString, tags: [TagsConstants.Ready]);
}
