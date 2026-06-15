using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

public class ApiKeyExternalServiceOptionsValidator : IValidateOptions<ApiKeyExternalServiceOptions>
{
    public ValidateOptionsResult Validate(string? name, ApiKeyExternalServiceOptions options)
    {
        var baseResult = ExternalServiceOptionsValidation.ValidateBase(name, options);
        if (baseResult.Failed)
        {
            return baseResult;
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeNotEmpty(name, nameof(ApiKeyExternalServiceOptions.ApiKey)));
        }

        return ValidateOptionsResult.Success;
    }
}
