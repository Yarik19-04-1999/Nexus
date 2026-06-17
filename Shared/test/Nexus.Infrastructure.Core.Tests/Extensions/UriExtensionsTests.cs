using Nexus.Infrastructure.Core.Extensions;

namespace Nexus.Infrastructure.Core.Tests.Extensions;

public class UriExtensionsTests
{
    [Fact]
    public void IsHttpOrHttps_WhenHttpScheme_ReturnsTrue()
    {
        var uri = new Uri("http://example.com");

        Assert.True(uri.IsHttpOrHttps());
    }

    [Fact]
    public void IsHttpOrHttps_WhenHttpsScheme_ReturnsTrue()
    {
        var uri = new Uri("https://example.com");

        Assert.True(uri.IsHttpOrHttps());
    }

    [Fact]
    public void IsHttpOrHttps_WhenFtpScheme_ReturnsFalse()
    {
        var uri = new Uri("ftp://example.com");

        Assert.False(uri.IsHttpOrHttps());
    }

    [Fact]
    public void IsHttpOrHttps_WhenFileScheme_ReturnsFalse()
    {
        var uri = new Uri("file:///some/path");

        Assert.False(uri.IsHttpOrHttps());
    }
}
