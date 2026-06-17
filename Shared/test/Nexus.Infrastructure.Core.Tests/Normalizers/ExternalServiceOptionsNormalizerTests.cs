using Nexus.Infrastructure.Core.Normalizers;
using Nexus.Infrastructure.Core.Options;

namespace Nexus.Infrastructure.Core.Tests.Normalizers;

public class ExternalServiceOptionsNormalizerTests
{
    [Fact]
    public void Normalize_WhenUrlHasTrailingSlash_TrimsIt()
    {
        var options = new ExternalServiceOptions { BaseUrl = "https://example.com/" };

        ExternalServiceOptionsNormalizer.Normalize(options);

        Assert.Equal("https://example.com", options.BaseUrl);
    }

    [Fact]
    public void Normalize_WhenUrlHasMultipleTrailingSlashes_TrimsAll()
    {
        var options = new ExternalServiceOptions { BaseUrl = "https://example.com///" };

        ExternalServiceOptionsNormalizer.Normalize(options);

        Assert.Equal("https://example.com", options.BaseUrl);
    }

    [Fact]
    public void Normalize_WhenUrlHasNoTrailingSlash_RemainsUnchanged()
    {
        var options = new ExternalServiceOptions { BaseUrl = "https://example.com" };

        ExternalServiceOptionsNormalizer.Normalize(options);

        Assert.Equal("https://example.com", options.BaseUrl);
    }
}
