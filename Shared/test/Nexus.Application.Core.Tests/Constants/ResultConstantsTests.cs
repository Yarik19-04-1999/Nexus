using Nexus.Application.Core.Constants;
using Nexus.Core.Tests.Constants;

namespace Nexus.Application.Core.Tests.Constants;

public class ResultConstantsTests
{
    private class Foo { }

    [Fact]
    public void NotFound_WithIntId_ReturnsFailureWithExpectedCodeAndMessage()
    {
        var result = ResultConstants.NotFound<Foo>(TestData.IntValue);

        Assert.True(result.HasError);
        Assert.Equal(CommonErrorCodes.NotFound, result.ErrorCode);
        Assert.Equal($"Foo with identifier {TestData.IntValue} not found.", result.ErrorMessage);
    }

    [Fact]
    public void NotFound_WithStringCode_ReturnsFailureWithExpectedCodeAndMessage()
    {
        var result = ResultConstants.NotFound<Foo>(TestData.StringValue);

        Assert.True(result.HasError);
        Assert.Equal(CommonErrorCodes.NotFound, result.ErrorCode);
        Assert.Equal($"Foo with code '{TestData.StringValue}' not found.", result.ErrorMessage);
    }

    [Fact]
    public void AlreadyExpired_WithIntId_ReturnsFailureWithExpectedCodeAndMessage()
    {
        var result = ResultConstants.AlreadyExpired<Foo>(TestData.IntValue);

        Assert.True(result.HasError);
        Assert.Equal(CommonErrorCodes.AlreadyExpired, result.ErrorCode);
        Assert.Equal($"Foo with identifier {TestData.IntValue} has already expired.", result.ErrorMessage);
    }

    [Fact]
    public void CodeAlreadyExists_WithStringCode_ReturnsFailureWithExpectedCodeAndMessage()
    {
        var result = ResultConstants.CodeAlreadyExists<Foo>(TestData.StringValue);

        Assert.True(result.HasError);
        Assert.Equal(CommonErrorCodes.CodeAlreadyExists, result.ErrorCode);
        Assert.Equal($"Foo with code '{TestData.StringValue}' already exists.", result.ErrorMessage);
    }
}
