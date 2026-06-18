using FluentAssertions;
using Nexus.Api.Core.Constants;
using Nexus.Core.Integration.Tests.Constants;
using Nexus.Core.Integration.Tests.Extensions;

namespace Nexus.Core.Integration.Tests.HealthChecks;

public abstract class NexusHealthCheckTests
{
    private readonly HttpClient _client;

    protected NexusHealthCheckTests(HttpClient client)
    {
        _client = client;
    }

    [Fact]
    public async Task HealthLive_ReturnsOk()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync(UrlConstants.Health.Live, ct);
        response.ShouldBeOk();
        var body = await response.ReadResponse(ct);
        body.Should().Be(HealthCheckConstants.Status.Healthy);
    }

    [Fact]
    public async Task HealthReady_ReturnsOk()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync(UrlConstants.Health.Ready, ct);
        response.ShouldBeOk();
        var report = await response.ReadHealthCheckReport(ct);
        report.Should().NotBeNull();
        report!.Status.Should().Be(HealthCheckConstants.Status.Healthy);
    }
}
