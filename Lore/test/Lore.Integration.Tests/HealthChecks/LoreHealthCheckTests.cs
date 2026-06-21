using Lore.Integration.Tests.Infrastructure;
using Nexus.Core.Integration.Tests.HealthChecks;

namespace Lore.Integration.Tests.HealthChecks;

public class LoreHealthCheckTests(LoreWebApplicationFactory factory)
    : NexusHealthCheckTests(factory.CreateClient()), IClassFixture<LoreWebApplicationFactory>;

public class LoreDbHealthCheckTests(LoreWebApplicationFactory factory)
    : NexusDbHealthCheckTests(factory.CreateClient()), IClassFixture<LoreWebApplicationFactory>
{
}
