using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Infrastructure.Sieve.Constants;
using Sieve.Models;
using Sieve.Services;

namespace Nexus.Infrastructure.Sieve.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSieve<TProcessor>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSection = SieveConfigConstants.DefaultSection)
        where TProcessor : class, ISieveProcessor
        => services
            .Configure<SieveOptions>(configuration.GetSection(configSection))
            .AddScoped<ISieveProcessor, TProcessor>();
}
