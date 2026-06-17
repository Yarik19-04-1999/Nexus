using Nexus.Application.Core.Constants;
using Nexus.Core.Tests.Constants;

namespace Nexus.Application.Core.Tests.Constants;

public class CommonErrorMessagesTests
{
    private class Foo { }

    [Fact]
    public void NotFound_WithIntId_ReturnsExpectedMessage()
    {
        var result = CommonErrorMessages.NotFound<Foo>(TestData.IntValue);

        Assert.Equal($"Foo with identifier {TestData.IntValue} not found.", result);
    }

    [Fact]
    public void NotFound_WithStringCode_ReturnsExpectedMessage()
    {
        var result = CommonErrorMessages.NotFound<Foo>(TestData.StringValue);

        Assert.Equal($"Foo with code '{TestData.StringValue}' not found.", result);
    }

    [Fact]
    public void AlreadyExpired_WithIntId_ReturnsExpectedMessage()
    {
        var result = CommonErrorMessages.AlreadyExpired<Foo>(TestData.IntValue);

        Assert.Equal($"Foo with identifier {TestData.IntValue} has already expired.", result);
    }

    [Fact]
    public void CodeAlreadyExists_WithStringCode_ReturnsExpectedMessage()
    {
        var result = CommonErrorMessages.CodeAlreadyExists<Foo>(TestData.StringValue);

        Assert.Equal($"Foo with code '{TestData.StringValue}' already exists.", result);
    }
}
