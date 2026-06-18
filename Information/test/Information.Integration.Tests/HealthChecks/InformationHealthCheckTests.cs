using Information.Integration.Tests.Infrastructure;
using Nexus.Core.Integration.Tests.HealthChecks;

namespace Information.Integration.Tests.HealthChecks;

public class InformationHealthCheckTests(InformationDbFactory factory)
    : NexusHealthCheckTests(factory.CreateClient()), IClassFixture<InformationDbFactory>;

public class InformationDbHealthCheckTests(InformationDbFactory factory)
    : NexusDbHealthCheckTests(factory.CreateClient()), IClassFixture<InformationDbFactory>
{
}
