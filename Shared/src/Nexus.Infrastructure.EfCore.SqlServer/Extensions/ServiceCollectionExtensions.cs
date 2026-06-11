using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.EfCore.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexusDbContext<T>(
        this IServiceCollection services,
        SqlServerOptions sqlServerOptions,
        IHostEnvironment environment) where T : DbContext
    {
        return services.AddDbContextPool<T>(options =>
        {
            options.UseSqlServer(sqlServerOptions.ConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
    }
}
