using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;
using Nexus.Infrastructure.Core.Constants;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

public class SqlServerOptionsValidator : IValidateOptions<SqlServerOptions>
{
    public ValidateOptionsResult Validate(string? name, SqlServerOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.Required(ConfigSectionConstants.SqlServer, nameof(SqlServerOptions.ConnectionString)));
        }

        return ValidateOptionsResult.Success;
    }
}
