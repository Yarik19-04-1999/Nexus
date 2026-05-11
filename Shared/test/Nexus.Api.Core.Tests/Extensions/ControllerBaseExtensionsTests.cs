using Microsoft.AspNetCore.Http;
using Nexus.Api.Core.Extensions;
using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Models;
using Nexus.AspNetCore.Core.Tests.Controllers;
using Nexus.Core.Tests.Constants;

namespace Nexus.Api.Core.Tests.Extensions;

public class ControllerBaseExtensionsTests
{
    private readonly TestController _controller = new();

    [Fact]
    public void DomainError_Returns418StatusCode()
    {
        var result = Result.Failure(TestErrorCodes.Default);

        var objectResult = _controller.DomainError(result);

        Assert.Equal(StatusCodes.Status418ImATeapot, objectResult.StatusCode);
    }

    [Fact]
    public void DomainError_ReturnsDomainErrorResponse()
    {
        var result = Result.Failure(TestErrorCodes.Default, TestErrorMessages.Default, canRetry: true);

        var objectResult = _controller.DomainError(result);

        var response = Assert.IsType<DomainErrorResponse>(objectResult.Value);
        Assert.Equal(TestErrorCodes.Default, response.ErrorCode);
        Assert.Equal(TestErrorMessages.Default, response.ErrorMessage);
        Assert.True(response.CanRetry);
    }
}
