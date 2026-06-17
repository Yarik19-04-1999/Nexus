using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.Options;

namespace Nexus.Api.Core.Tests.Extensions;

public class NexusOptionsExtensionsTests
{
    [Fact]
    public void HasRequestTimeout_WhenTimeoutIsZero_ReturnsFalse()
    {
        var options = new NexusOptions { RequestTimeout = TimeSpan.Zero };

        var result = options.HasRequestTimeout();

        Assert.False(result);
    }

    [Fact]
    public void HasRequestTimeout_WhenTimeoutIsNegative_ReturnsFalse()
    {
        var options = new NexusOptions { RequestTimeout = TimeSpan.FromSeconds(-1) };

        var result = options.HasRequestTimeout();

        Assert.False(result);
    }

    [Fact]
    public void HasRequestTimeout_WhenTimeoutIsPositive_ReturnsTrue()
    {
        var options = new NexusOptions { RequestTimeout = TimeSpan.FromSeconds(30) };

        var result = options.HasRequestTimeout();

        Assert.True(result);
    }
}
