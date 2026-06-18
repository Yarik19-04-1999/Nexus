using Microsoft.AspNetCore.Hosting;
using Nexus.Core.Integration.Tests.Fixtures;
using Nexus.Infrastructure.Core.Constants;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Core.Integration.Tests.Factories;

public class NexusSqlWebApplicationFactory<TProgram>(NexusSqlFixture sqlFixture)
    : NexusWebApplicationFactoryWithoutHostedServices<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting($"{ConfigSectionConstants.SqlServer}:{nameof(SqlServerOptions.ConnectionString)}", sqlFixture.ConnectionString);
        base.ConfigureWebHost(builder);
    }
}
