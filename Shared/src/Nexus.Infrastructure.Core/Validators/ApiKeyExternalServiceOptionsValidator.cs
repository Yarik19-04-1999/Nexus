using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

public class ApiKeyExternalServiceOptionsValidator : IValidateOptions<ApiKeyExternalServiceOptions>
{
    public ValidateOptionsResult Validate(string? name, ApiKeyExternalServiceOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            return ValidateOptionsResult.Fail($"[{name}] {nameof(ApiKeyExternalServiceOptions.BaseUrl)} must not be empty.");
        }

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return ValidateOptionsResult.Fail($"[{name}] {nameof(ApiKeyExternalServiceOptions.BaseUrl)} must be a valid HTTP or HTTPS URL.");
        }

        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"[{name}] {nameof(ApiKeyExternalServiceOptions.Timeout)} must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            return ValidateOptionsResult.Fail($"[{name}] {nameof(ApiKeyExternalServiceOptions.ApiKey)} must not be empty.");
        }

        return ValidateOptionsResult.Success;
    }
}
