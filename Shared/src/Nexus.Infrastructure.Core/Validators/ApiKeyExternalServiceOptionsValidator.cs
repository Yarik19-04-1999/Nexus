using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

public class ApiKeyExternalServiceOptionsValidator : IValidateOptions<ApiKeyExternalServiceOptions>
{
    private readonly ExternalServiceOptionsValidator _baseValidator = new();

    public ValidateOptionsResult Validate(string? name, ApiKeyExternalServiceOptions options)
    {
        var baseResult = _baseValidator.Validate(name, options);
        if (baseResult.Failed)
        {
            return baseResult;
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            return ValidateOptionsResult.Fail($"[{name}] {nameof(ApiKeyExternalServiceOptions.ApiKey)} must not be empty.");
        }

        return ValidateOptionsResult.Success;
    }
}
