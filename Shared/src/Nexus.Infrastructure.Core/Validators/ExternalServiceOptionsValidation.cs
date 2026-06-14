using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

internal static class ExternalServiceOptionsValidation
{
    internal static ValidateOptionsResult ValidateBase(string? name, ExternalServiceOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            return ValidateOptionsResult.Fail(
                $"[{name}] {nameof(ExternalServiceOptions.BaseUrl)} must not be empty.");
        }

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return ValidateOptionsResult.Fail(
                $"[{name}] {nameof(ExternalServiceOptions.BaseUrl)} must be a valid HTTP or HTTPS URL.");
        }

        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                $"[{name}] {nameof(ExternalServiceOptions.Timeout)} must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
