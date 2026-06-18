using FluentAssertions;
using Nexus.Api.Core.Constants;
using Nexus.Core.Integration.Tests.Constants;
using Nexus.Core.Integration.Tests.Extensions;

namespace Nexus.Core.Integration.Tests.HealthChecks;

public abstract class NexusDbHealthCheckTests
{
    private readonly HttpClient _client;

    protected NexusDbHealthCheckTests(HttpClient client)
    {
        _client = client;
    }

    [Fact]
    public async Task HealthReady_DbContextEntry_IsHealthy()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync(UrlConstants.Health.Ready, ct);
        response.ShouldBeOk();
        var report = await response.ReadHealthCheckReport(ct);
        report.Should().NotBeNull();
        report!.Entries.Values.Should().AllSatisfy(entry =>
        {
            entry.Status.Should().Be(HealthCheckConstants.Status.Healthy);
            entry.Tags.Should().ContainSingle(TagsConstants.HealthChecks.Ready);
        });
    }
}
