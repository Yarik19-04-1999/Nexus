using Microsoft.Extensions.Options;

namespace Nexus.Application.Core.Extensions;

public static class ValidateOptionsExtensions
{
    public static void ValidateAndThrow<T>(this IValidateOptions<T> validator, T options) where T : class
    {
        var result = validator.Validate(null, options);

        if (result.Failed)
        {
            throw new OptionsValidationException(
                Options.DefaultName,
                typeof(T),
                result.Failures!);
        }
    }
}
