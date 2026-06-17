using Microsoft.Extensions.Options;
using Nexus.Application.Core.Constants;

namespace Information.Api.Bot.Options;

public class BotOptionsValidator : IValidateOptions<BotOptions>
{
    public ValidateOptionsResult Validate(string? name, BotOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Token))
        {
            return ValidateOptionsResult.Fail(OptionsErrorMessages.MustBeNotEmpty(name, nameof(BotOptions.Token)));
        }

        return ValidateOptionsResult.Success;
    }
}
