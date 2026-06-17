using Information.Application.Models.Options;
using Microsoft.Extensions.Options;

namespace Information.Application.Validators;

internal class ExchangeRateCacheOptionsValidator : IValidateOptions<ExchangeRateCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, ExchangeRateCacheOptions options)
    {
        if (options.CacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"{nameof(ExchangeRateCacheOptions.CacheExpiration)} must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
