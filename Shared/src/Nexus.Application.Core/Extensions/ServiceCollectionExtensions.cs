using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Nexus.Application.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureOptions<TOptions, TValidator>(
        this IServiceCollection services,
        IConfigurationSection section)
        where TOptions : class
        where TValidator : class, IValidateOptions<TOptions>
        => services.ConfigureOptions<TOptions, TValidator>(Options.DefaultName, section);

    public static IServiceCollection ConfigureOptions<TOptions, TValidator>(
        this IServiceCollection services,
        string name,
        IConfigurationSection section)
        where TOptions : class
        where TValidator : class, IValidateOptions<TOptions>
    {
        services.Configure<TOptions>(name, section);
        services.TryAddSingleton<IValidateOptions<TOptions>, TValidator>();
        services.AddOptions<TOptions>(name).ValidateOnStart();
        return services;
    }
}
