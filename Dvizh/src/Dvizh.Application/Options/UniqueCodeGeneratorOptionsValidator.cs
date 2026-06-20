using Microsoft.Extensions.Options;

namespace Dvizh.Application.Options;

public class UniqueCodeGeneratorOptionsValidator : IValidateOptions<UniqueCodeGeneratorOptions>
{
    public ValidateOptionsResult Validate(string? name, UniqueCodeGeneratorOptions options)
    {
        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"{nameof(UniqueCodeGeneratorOptions.Timeout)} must be a positive value.");
        }

        return ValidateOptionsResult.Success;
    }
}
