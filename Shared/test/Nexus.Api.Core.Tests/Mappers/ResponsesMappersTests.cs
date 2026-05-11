using Nexus.Api.Core.Mappers;
using Nexus.Application.Core.Models;
using Nexus.Core.Tests.Constants;

namespace Nexus.Api.Core.Tests.Mappers;

public class ResponsesMappersTests
{
    [Fact]
    public void MapToDomainErrorResponse_MapsErrorCode()
    {
        var result = Result.Failure(TestErrorCodes.Default);

        var response = result.MapToDomainErrorResponse();

        Assert.Equal(TestErrorCodes.Default, response.ErrorCode);
    }

    [Fact]
    public void MapToDomainErrorResponse_MapsAllProperties()
    {
        var result = Result.Failure(TestErrorCodes.Default, TestErrorMessages.Default, canRetry: true);

        var response = result.MapToDomainErrorResponse();

        Assert.Equal(TestErrorCodes.Default, response.ErrorCode);
        Assert.Equal(TestErrorMessages.Default, response.ErrorMessage);
        Assert.True(response.CanRetry);
    }

    [Fact]
    public void MapToDomainErrorResponse_WithoutOptionalFields_SetsDefaults()
    {
        var result = Result.Failure(TestErrorCodes.Default);

        var response = result.MapToDomainErrorResponse();

        Assert.Null(response.ErrorMessage);
        Assert.False(response.CanRetry);
    }
}
