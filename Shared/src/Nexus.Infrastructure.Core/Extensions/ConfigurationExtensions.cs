using Microsoft.Extensions.Configuration;
using Nexus.Application.Core.Extensions;
using Nexus.Infrastructure.Core.Constants;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Extensions;

public static class ConfigurationExtensions
{
    public static SqlServerOptions GetSqlServerOptions(this IConfiguration configuration)
        => configuration.GetAndValidateRequiredOptions(
            OptionsConstants.SqlServer.SectionName,
            new SqlServerOptionsValidator());
}
