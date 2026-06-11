using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Constants;

namespace Nexus.Infrastructure.Core.Options;

public class SqlServerOptionsValidator : IValidateOptions<SqlServerOptions>
{
    public ValidateOptionsResult Validate(string? name, SqlServerOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            return ValidateOptionsResult.Fail($"{OptionsConstants.SqlServer.SectionName}.{nameof(SqlServerOptions.ConnectionString)} is required.");

        return ValidateOptionsResult.Success;
    }
}
