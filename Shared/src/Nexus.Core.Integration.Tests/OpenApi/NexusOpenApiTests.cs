using Nexus.Api.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;

namespace Nexus.Core.Integration.Tests.OpenApi;

public abstract class NexusOpenApiTests
{
    private readonly HttpClient _enabledClient;
    private readonly HttpClient _disabledClient;
    private readonly IReadOnlyList<string> _documentNames;

    protected NexusOpenApiTests(HttpClient enabledClient, HttpClient disabledClient, IReadOnlyList<string> documentNames)
    {
        _enabledClient = enabledClient;
        _disabledClient = disabledClient;
        _documentNames = documentNames;
    }

    private IEnumerable<string> AllOpenApiUrls =>
        _documentNames.SelectMany(name => new[]
        {
            UrlConstants.OpenApi.Document(name),
            UrlConstants.OpenApi.ScalarUi(name),
        });

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
