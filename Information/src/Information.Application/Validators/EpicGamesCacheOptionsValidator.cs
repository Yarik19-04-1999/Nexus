using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;

namespace Information.Application.Validators;

internal class EpicGamesCacheOptionsValidator : IValidateOptions<EpicGamesCacheOptions>
{
    public ValidateOptionsResult Validate(string? name, EpicGamesCacheOptions options)
    {
        if (options.CacheExpiration <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeGreaterThanZero(nameof(EpicGamesCacheOptions), nameof(EpicGamesCacheOptions.CacheExpiration)));
        }

        return ValidateOptionsResult.Success;
    }
}
