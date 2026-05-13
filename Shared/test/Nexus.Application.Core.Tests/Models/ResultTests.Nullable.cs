using Nexus.Application.Core.Models;
using Nexus.Core.Tests.Constants;

namespace Nexus.Application.Core.Tests.Models;

public class ResultTests_Nullable
{
    [Fact]
    public void Success_WithData_ReturnsSuccessResult()
    {
        var result = NullableResult<int>.Success(TestData.IntValue);

        Assert.True(result.IsSuccess);
        Assert.False(result.HasError);
        Assert.Equal(TestData.IntValue, result.Data);
    }

    [Fact]
    public void Success_WithNullData_ReturnsSuccessResult()
    {
        var result = NullableResult<string>.Success(null);

        Assert.True(result.IsSuccess);
        Assert.False(result.HasError);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Success_WithoutData_ReturnsSuccessResultWithDefault()
    {
        var result = NullableResult<string>.Success();

        Assert.True(result.IsSuccess);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Success_WithCanRetry_SetsCanRetryTrue()
    {
        var result = NullableResult<int>.Success(TestData.IntValue, canRetry: true);

        Assert.True(result.IsSuccess);
        Assert.True(result.CanRetry);
    }

    [Fact]
    public void Failure_ReturnsErrorResult()
    {
        var result = NullableResult<string>.Failure(TestErrorCodes.Default);

        Assert.True(result.HasError);
        Assert.False(result.IsSuccess);
        Assert.Equal(TestErrorCodes.Default, result.ErrorCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Failure_WithAllParameters_SetsAllProperties()
    {
        var result = NullableResult<int>.Failure(
            TestErrorCodes.Default,
            TestErrorMessages.Default,
            canRetry: true);

        Assert.True(result.HasError);
        Assert.Equal(TestErrorCodes.Default, result.ErrorCode);
        Assert.Equal(TestErrorMessages.Default, result.ErrorMessage);
        Assert.True(result.CanRetry);
    }

    [Fact]
    public void Success_AlwaysReturnsNewInstance()
    {
        var first = NullableResult<int>.Success(TestData.IntValue);
        var second = NullableResult<int>.Success(TestData.IntValue);

        Assert.NotSame(first, second);
    }
}
