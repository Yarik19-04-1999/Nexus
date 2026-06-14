using Information.Application.Models.Options;
using Microsoft.Extensions.Options;

namespace Information.Application.Validators;

internal class EpicGamesCacheOptionsValidator : IValidateOptions<EpicGamesCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, EpicGamesCacheOptions options)
    {
        if (options.CacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"{nameof(EpicGamesCacheOptions.CacheExpiration)} must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
