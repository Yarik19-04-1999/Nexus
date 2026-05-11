using Nexus.Api.Core.ViewModels;
using Nexus.Core.Tests.Constants;

namespace Nexus.Api.Core.Tests.ViewModels;

public class DomainErrorResponseTests
{
    [Fact]
    public void Constructor_WithRequiredOnly_SetsDefaults()
    {
        var response = new DomainErrorResponse(TestErrorCodes.Default);

        Assert.Equal(TestErrorCodes.Default, response.ErrorCode);
        Assert.Null(response.ErrorMessage);
        Assert.False(response.CanRetry);
    }

    [Fact]
    public void Constructor_WithAllParameters_SetsAllProperties()
    {
        var response = new DomainErrorResponse(TestErrorCodes.Default, TestErrorMessages.Default, CanRetry: true);

        Assert.Equal(TestErrorCodes.Default, response.ErrorCode);
        Assert.Equal(TestErrorMessages.Default, response.ErrorMessage);
        Assert.True(response.CanRetry);
    }
}
