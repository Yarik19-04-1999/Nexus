using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;

namespace Dvizh.Application.Options;

public class InviteCodeGenerationOptionsValidator : IValidateOptions<InviteCodeGenerationOptions>
{
    public ValidateOptionsResult Validate(string? name, InviteCodeGenerationOptions options)
    {
        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                OptionsErrorMessages.MustBeGreaterThanZero(name, nameof(InviteCodeGenerationOptions.Timeout)));
        }

        return ValidateOptionsResult.Success;
    }
}
