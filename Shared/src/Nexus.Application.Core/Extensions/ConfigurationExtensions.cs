using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Nexus.Application.Core.Extensions;

public static class ConfigurationExtensions
{
    public static T? GetOptions<T>(this IConfiguration configuration, string sectionName)
        => configuration.GetSection(sectionName).Get<T>();

    public static T GetRequiredOptions<T>(
        this IConfiguration configuration,
        string sectionName)
    {
        var options = configuration.GetOptions<T>(sectionName)
            ?? throw new InvalidOperationException($"Section '{sectionName}' is missing from configuration.");

        return options;
    }

    public static T GetAndValidateRequiredOptions<T>(
        this IConfiguration configuration,
        string sectionName,
        IValidateOptions<T> validator) where T : class
    {
        var options = configuration.GetRequiredOptions<T>(sectionName);
        validator.ValidateAndThrow(options);

        return options;
    }
}
