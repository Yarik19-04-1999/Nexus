using Information.Application.Models.Options;
using Microsoft.Extensions.Options;

namespace Information.Application.Validators;

internal class WeatherCacheOptionsValidator : IValidateOptions<WeatherCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, WeatherCacheOptions options)
    {
        if (options.CacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"{nameof(WeatherCacheOptions.CacheExpiration)} must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
