using Information.Application.Models.Options;
using Microsoft.Extensions.Options;

namespace Information.Application.Validators;

internal class WeatherCacheOptionsValidator : IValidateOptions<WeatherCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, WeatherCacheOptions options)
    {
        if (options.HourlyCacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"{nameof(WeatherCacheOptions.HourlyCacheExpiration)} must be greater than zero.");
        }

        if (options.DailyCacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"{nameof(WeatherCacheOptions.DailyCacheExpiration)} must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
