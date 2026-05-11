using Nexus.Application.Core.Models;
using Nexus.Core.Tests.Constants;

namespace Nexus.Application.Core.Tests.Models;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.HasError);
        Assert.Null(result.ErrorCode);
        Assert.Null(result.ErrorMessage);
        Assert.False(result.CanRetry);
    }

    [Fact]
    public void Success_WithCanRetry_SetsCanRetryTrue()
    {
        var result = Result.Success(canRetry: true);

        Assert.True(result.IsSuccess);
        Assert.True(result.CanRetry);
    }

    [Fact]
    public void Success_ReturnsCachedInstance()
    {
        var first = Result.Success();
        var second = Result.Success();

        Assert.Same(first, second);
    }

    [Fact]
    public void Success_WithCanRetry_ReturnsCachedInstance()
    {
        var first = Result.Success(canRetry: true);
        var second = Result.Success(canRetry: true);

        Assert.Same(first, second);
    }

    [Fact]
    public void Failure_ReturnsErrorResult()
    {
        var result = Result.Failure(TestErrorCodes.Default);

        Assert.True(result.HasError);
        Assert.False(result.IsSuccess);
        Assert.Equal(TestErrorCodes.Default, result.ErrorCode);
        Assert.Null(result.ErrorMessage);
        Assert.False(result.CanRetry);
    }

    [Fact]
    public void Failure_WithAllParameters_SetsAllProperties()
    {
        var result = Result.Failure(
            TestErrorCodes.Default,
            TestErrorMessages.Default,
            canRetry: true);

        Assert.True(result.HasError);
        Assert.Equal(TestErrorCodes.Default, result.ErrorCode);
        Assert.Equal(TestErrorMessages.Default, result.ErrorMessage);
        Assert.True(result.CanRetry);
    }

    [Fact]
    public void Failure_AlwaysReturnsNewInstance()
    {
        var first = Result.Failure(TestErrorCodes.Default);
        var second = Result.Failure(TestErrorCodes.Default);

        Assert.NotSame(first, second);
    }
}
