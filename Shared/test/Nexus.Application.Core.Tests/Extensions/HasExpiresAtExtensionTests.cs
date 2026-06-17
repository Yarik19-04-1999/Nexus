using Nexus.Application.Core.Extensions;
using Nexus.Application.Core.Interfaces;

namespace Nexus.Application.Core.Tests.Extensions;

public class HasExpiresAtExtensionTests
{
    private class FakeEntity : IHasExpiresAt
    {
        public DateTime? ExpiresAt { get; set; }
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsNull_ReturnsFalse()
    {
        var entity = new FakeEntity { ExpiresAt = null };

        var result = entity.IsExpired();

        Assert.False(result);
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsInPast_ReturnsTrue()
    {
        var entity = new FakeEntity { ExpiresAt = DateTime.UtcNow.AddSeconds(-1) };

        var result = entity.IsExpired();

        Assert.True(result);
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsInFuture_ReturnsFalse()
    {
        var entity = new FakeEntity { ExpiresAt = DateTime.UtcNow.AddSeconds(60) };

        var result = entity.IsExpired();

        Assert.False(result);
    }
}
