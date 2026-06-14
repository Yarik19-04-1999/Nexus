using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

public class ExternalServiceOptionsValidator : IValidateOptions<ExternalServiceOptions>
{
    public ValidateOptionsResult Validate(string? name, ExternalServiceOptions options)
        => ExternalServiceOptionsValidation.ValidateBase(name, options);
}
