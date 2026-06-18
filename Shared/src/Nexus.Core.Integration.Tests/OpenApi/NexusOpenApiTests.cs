using Microsoft.AspNetCore.Mvc.Testing;
using Nexus.Api.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;

namespace Nexus.Core.Integration.Tests.OpenApi;

public abstract class NexusOpenApiTests<TProgram, TFactory> : IClassFixture<TFactory>
    where TProgram : class
    where TFactory : WebApplicationFactory<TProgram>
{
    private readonly HttpClient _enabledClient;
    private readonly HttpClient _disabledClient;
    private readonly IReadOnlyList<string> _documentNames;

    private IEnumerable<string> AllOpenApiUrls =>
        _documentNames.SelectMany(name => new[]
        {
            UrlConstants.OpenApi.Document(name),
            UrlConstants.OpenApi.ScalarUi(name),
        });

    protected NexusOpenApiTests(TFactory factory)
    {
        _enabledClient = factory.CreateClient();
        _disabledClient = factory
            .WithConfiguration($"{ConfigSectionConstants.Configuration}:UseOpenApi", "false")
            .CreateClient();
        _documentNames = factory.GetOpenApiDocumentNames();
    }

    [Fact]
    public async Task OpenApi_Enabled_AllEndpointsReturnOk()
    {
        var ct = TestContext.Current.CancellationToken;

        foreach (var url in AllOpenApiUrls)
        {
            var response = await _enabledClient.GetAsync(url, ct);
            response.ShouldBeOk();
        }
    }

    [Fact]
    public async Task OpenApi_Disabled_AllEndpointsReturnNotFound()
    {
        var ct = TestContext.Current.CancellationToken;

        foreach (var url in AllOpenApiUrls)
        {
            var response = await _disabledClient.GetAsync(url, ct);
            response.ShouldBeNotFound();
        }
    }
}
