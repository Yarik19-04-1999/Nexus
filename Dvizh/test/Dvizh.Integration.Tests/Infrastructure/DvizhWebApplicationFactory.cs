using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nexus.Core.Integration.Tests.Factories;
using Nexus.Infrastructure.Core.Constants;

namespace Dvizh.Integration.Tests.Infrastructure;

/// <summary>
/// Class-scoped fixture. Receives DvizhSqlFixture (assembly fixture) via xunit.v3 constructor injection.
/// Injects the container connection string via ConfigureHostConfiguration — this runs before
/// Program.cs service registration, so GetSqlServerOptions() sees the real connection string.
/// </summary>
public class DvizhWebApplicationFactory(DvizhSqlFixture sqlFixture) : NexusWebApplicationFactoryWithoutHostedServices<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting($"{OptionsConstants.SqlServer.SectionName}:ConnectionString", sqlFixture.ConnectionString);
        base.ConfigureWebHost(builder);
    }
}
