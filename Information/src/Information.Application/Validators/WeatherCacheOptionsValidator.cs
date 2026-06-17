using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;

namespace Information.Application.Validators;

internal class WeatherCacheOptionsValidator : IValidateOptions<WeatherCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, WeatherCacheOptions options)
    {
        if (options.HourlyCacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeGreaterThanZero(nameof(WeatherCacheOptions), nameof(WeatherCacheOptions.HourlyCacheExpiration)));
        }

        if (options.DailyCacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeGreaterThanZero(nameof(WeatherCacheOptions), nameof(WeatherCacheOptions.DailyCacheExpiration)));
        }

        return ValidateOptionsResult.Success;
    }
}
