using Microsoft.Extensions.Configuration;
using Nexus.Application.Core.Extensions;
using Nexus.Infrastructure.Core.Constants;
using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;

namespace Nexus.Infrastructure.Core.Extensions;

public static class ConfigurationExtensions
{
    public static SqlServerOptions GetSqlServerOptions(this IConfiguration configuration)
        => configuration.GetAndValidateRequiredOptions(
            ConfigSectionConstants.SqlServer,
            new SqlServerOptionsValidator());
}
