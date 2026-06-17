using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Core.Integration.Tests.Factories;

namespace Information.Integration.Tests.Infrastructure;

public class InformationWebApplicationFactory : NexusWebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove all hosted services:
            // - BotPollingService (prevents Telegram connection in tests)
            // - ValidateOnStart services (options validators run at host startup)
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
