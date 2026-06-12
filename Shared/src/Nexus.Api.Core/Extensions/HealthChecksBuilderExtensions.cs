using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Api.Core.Extensions;

public static class HealthChecksBuilderExtensions
{
    public static IHealthChecksBuilder AddNexusSqlServerHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        => builder.AddSqlServer(connectionString, tags: ["ready"]);
}
