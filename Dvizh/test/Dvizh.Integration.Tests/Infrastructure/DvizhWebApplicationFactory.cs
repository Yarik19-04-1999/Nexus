using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Infrastructure.Core.Constants;
using Nexus.Core.Integration.Tests.Factories;

namespace Dvizh.Integration.Tests.Infrastructure;

/// <summary>
/// Class-scoped fixture. Receives DvizhSqlFixture (assembly fixture) via xunit.v3 constructor injection.
/// Injects the container connection string via ConfigureHostConfiguration — this runs before
/// Program.cs service registration, so GetSqlServerOptions() sees the real connection string.
/// </summary>
public class DvizhWebApplicationFactory(DvizhSqlFixture sqlFixture) : NexusWebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(appConfig =>
        {
            appConfig = appConfig.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{OptionsConstants.SqlServer.SectionName}:ConnectionString"] = sqlFixture.ConnectionString
            });
        });

        builder.ConfigureServices(services =>
        {
            var hostedServices = services
                .Where(d => d.ServiceType == typeof(IHostedService))
                .ToList();
            foreach (var descriptor in hostedServices)
            {
                services.Remove(descriptor);
            }
        });

        base.ConfigureWebHost(builder);
    }
}
