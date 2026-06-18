using Microsoft.AspNetCore.Hosting;
using Nexus.Core.Integration.Tests.Factories;
using Nexus.Infrastructure.Core.Constants;
using Nexus.Infrastructure.Core.Options;

namespace Information.Integration.Tests.Infrastructure;

public class InformationDbFactory(InformationSqlFixture sqlFixture) : NexusWebApplicationFactoryWithoutHostedServices<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting($"{ConfigSectionConstants.SqlServer}:{nameof(SqlServerOptions.ConnectionString)}", sqlFixture.ConnectionString);
        base.ConfigureWebHost(builder);
    }
}
