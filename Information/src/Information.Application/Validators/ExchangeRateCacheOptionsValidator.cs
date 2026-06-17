using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;

namespace Information.Application.Validators;

internal class ExchangeRateCacheOptionsValidator : IValidateOptions<ExchangeRateCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, ExchangeRateCacheOptions options)
    {
        if (options.CacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeGreaterThanZero(nameof(ExchangeRateCacheOptions), nameof(ExchangeRateCacheOptions.CacheExpiration)));
        }

        return ValidateOptionsResult.Success;
    }
}
