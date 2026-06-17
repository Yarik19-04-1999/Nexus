using Nexus.Api.Core.Mappers;
using Nexus.Application.Core.Exceptions;

namespace Nexus.Api.Core.Tests.Mappers;

public class DomainExceptionMapTests
{
    [Fact]
    public void Map_DomainException_MapsErrorCodeAndMessage()
    {
        var exception = new DomainException("E001", "Bad thing");

        var response = exception.Map();

        Assert.Equal("E001", response.ErrorCode);
        Assert.Equal("Bad thing", response.ErrorMessage);
    }

    [Fact]
    public void Map_DomainException_PropagatesCanRetry()
    {
        var exception = new DomainException("E002", "Some error", canRetry: true);

        var response = exception.Map();

        Assert.True(response.CanRetry);
    }

    [Fact]
    public void Map_DomainException_CanRetryFalseByDefault()
    {
        var exception = new DomainException("E003", "Some error");

        var response = exception.Map();

        Assert.False(response.CanRetry);
    }
}
