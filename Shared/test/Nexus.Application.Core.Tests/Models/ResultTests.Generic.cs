using Nexus.Application.Core.Models;
using Nexus.Core.Tests.Constants;

namespace Nexus.Application.Core.Tests.Models;

public class ResultTests_Generic
{
    [Fact]
    public void Success_ReturnsSuccessResultWithData()
    {
        var result = Result<int>.Success(TestData.IntValue);

        Assert.True(result.IsSuccess);
        Assert.False(result.HasError);
        Assert.Equal(TestData.IntValue, result.Data);
        Assert.Null(result.ErrorCode);
        Assert.Null(result.ErrorMessage);
        Assert.False(result.CanRetry);
    }

    [Fact]
    public void Success_WithCanRetry_SetsCanRetryTrue()
    {
        var result = Result<int>.Success(TestData.IntValue, canRetry: true);

        Assert.True(result.IsSuccess);
        Assert.True(result.CanRetry);
        Assert.Equal(TestData.IntValue, result.Data);
    }

    [Fact]
    public void Success_WithStringData_ReturnsCorrectData()
    {
        var result = Result<string>.Success(TestData.StringValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(TestData.StringValue, result.Data);
    }

    [Fact]
    public void Failure_ReturnsErrorResultWithNullData()
    {
        var result = Result<int>.Failure(TestErrorCodes.Default);

        Assert.True(result.HasError);
        Assert.False(result.IsSuccess);
        Assert.Equal(TestErrorCodes.Default, result.ErrorCode);
        Assert.Equal(default, result.Data);
    }

    [Fact]
    public void Failure_WithAllParameters_SetsAllProperties()
    {
        var result = Result<string>.Failure(
            TestErrorCodes.Default,
            TestErrorMessages.Default,
            canRetry: true);

        Assert.True(result.HasError);
        Assert.Equal(TestErrorCodes.Default, result.ErrorCode);
        Assert.Equal(TestErrorMessages.Default, result.ErrorMessage);
        Assert.True(result.CanRetry);
        Assert.Null(result.Data);
    }

    [Fact]
    public void IsSuccess_WhenTrue_DataIsNotNull()
    {
        var result = Result<string>.Success(TestData.StringValue);

        if (result.IsSuccess)
        {
            string data = result.Data;
            Assert.Equal(TestData.StringValue, data);
        }
    }

    [Fact]
    public void HasError_WhenTrue_ErrorCodeIsNotNull()
    {
        var result = Result<int>.Failure(TestErrorCodes.Default);

        if (result.HasError)
        {
            string errorCode = result.ErrorCode;
            Assert.Equal(TestErrorCodes.Default, errorCode);
        }
    }

    [Fact]
    public void Success_AlwaysReturnsNewInstance()
    {
        var first = Result<int>.Success(TestData.IntValue);
        var second = Result<int>.Success(TestData.IntValue);

        Assert.NotSame(first, second);
    }
}
