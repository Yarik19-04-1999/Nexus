using Nexus.Application.Core.Constants;

namespace Nexus.Application.Core.Tests.Constants;

public class OptionsErrorMessagesTests
{
    [Fact]
    public void Required_WithSectionAndProperty_ReturnsExpectedMessage()
    {
        var result = OptionsErrorMessages.Required("SqlServer", "ConnectionString");

        Assert.Equal("SqlServer.ConnectionString is required.", result);
    }

    [Fact]
    public void Required_WithFullPath_ReturnsExpectedMessage()
    {
        var result = OptionsErrorMessages.Required("SqlServer.ConnectionString");

        Assert.Equal("SqlServer.ConnectionString is required.", result);
    }

    [Fact]
    public void MustBeNotEmpty_WithNullName_ReturnsExpectedMessage()
    {
        var result = OptionsErrorMessages.MustBeNotEmpty(null, "Token");

        Assert.Equal("[null] Token must not be empty.", result);
    }

    [Fact]
    public void MustBeNotEmpty_WithName_ReturnsExpectedMessage()
    {
        var result = OptionsErrorMessages.MustBeNotEmpty("BotOptions", "Token");

        Assert.Equal("[BotOptions] Token must not be empty.", result);
    }

    [Fact]
    public void MustBeGreaterThanZero_ReturnsExpectedMessage()
    {
        var result = OptionsErrorMessages.MustBeGreaterThanZero("MyOptions", "Timeout");

        Assert.Equal("[MyOptions] Timeout must be greater than zero.", result);
    }

    [Fact]
    public void MustBeValidHttpOrHttps_ReturnsExpectedMessage()
    {
        var result = OptionsErrorMessages.MustBeValidHttpOrHttps("MyOptions", "BaseUrl");

        Assert.Equal("[MyOptions] BaseUrl must be a valid HTTP or HTTPS URL.", result);
    }
}
