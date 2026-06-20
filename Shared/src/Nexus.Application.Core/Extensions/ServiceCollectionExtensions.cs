using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Services;
using System.Xml.Linq;

namespace Nexus.Application.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUniqueCodeService(this IServiceCollection services)
        => services.AddSingleton<IUniqueCodeService, UniqueCodeService>();

    public static OptionsBuilder<TOptions> ConfigureOptions<TOptions>(
        this IServiceCollection services,
        string name,
        string configurationPath)
        where TOptions : class
        => services.AddOptions<TOptions>(name).BindConfiguration(configurationPath);

    public static OptionsBuilder<TOptions> WithValidator<TOptions, TValidator>(this OptionsBuilder<TOptions> builder)
        where TOptions : class
        where TValidator : class, IValidateOptions<TOptions>
    {
        builder.Services.TryAddSingleton<IValidateOptions<TOptions>, TValidator>();
        return builder;
    }

    public static OptionsBuilder<TOptions> WithPostConfigure<TOptions, TPostConfigure>(this OptionsBuilder<TOptions> builder)
        where TOptions : class
        where TPostConfigure : class, IPostConfigureOptions<TOptions>
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<TOptions>, TPostConfigure>();
        return builder;
    }
}
