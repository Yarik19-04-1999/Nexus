using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;
using Nexus.Infrastructure.Core.Extensions;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Validators;

public class ExternalServiceOptionsValidator : IValidateOptions<ExternalServiceOptions>
{
    public ValidateOptionsResult Validate(string? name, ExternalServiceOptions options)
        => ExternalServiceOptionsValidator.ValidateBase(name, options);

    public static ValidateOptionsResult ValidateBase(string? name, ExternalServiceOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeNotEmpty(name, nameof(ExternalServiceOptions.BaseUrl)));
        }

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri) || !uri.IsHttpOrHttps())
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeValidHttpOrHttps(name, nameof(ExternalServiceOptions.BaseUrl)));
        }

        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeGreaterThanZero(name, nameof(ExternalServiceOptions.Timeout)));
        }

        return ValidateOptionsResult.Success;
    }
}
