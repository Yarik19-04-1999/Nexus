using Lore.Application.Interfaces.Stores;
using Lore.Infrastructure.DbContexts;
using Lore.Infrastructure.Services;
using Lore.Infrastructure.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Infrastructure.Core.Extensions;
using Nexus.Infrastructure.EfCore.SqlServer.Extensions;
using Nexus.Infrastructure.Sieve.Extensions;

namespace Lore.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
        => services
            .AddLoreDbContext(configuration, environment)
            .AddStores()
            .AddSieve<LoreSieveProcessor>(configuration);

    private static IServiceCollection AddLoreDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var sqlServerOptions = configuration.GetSqlServerOptions();
        return services.AddNexusDbContext<LoreDbContext>(sqlServerOptions, environment);
    }

    private static IServiceCollection AddStores(this IServiceCollection services)
        => services.AddScoped<ILoreStore, LoreStore>();
}
