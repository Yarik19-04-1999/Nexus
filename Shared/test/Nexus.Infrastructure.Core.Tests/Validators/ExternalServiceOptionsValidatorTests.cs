using Nexus.Application.Core.Constants;
using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;

namespace Nexus.Infrastructure.Core.Tests.Validators;

public class ExternalServiceOptionsValidatorTests
{
    private readonly ExternalServiceOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenBaseUrlIsEmpty_ReturnsFailedWithExpectedMessage()
    {
        var options = new ExternalServiceOptions { BaseUrl = string.Empty };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeNotEmpty(null, nameof(ExternalServiceOptions.BaseUrl)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenBaseUrlIsNull_ReturnsFailedWithExpectedMessage()
    {
        var options = new ExternalServiceOptions { BaseUrl = null! };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeNotEmpty(null, nameof(ExternalServiceOptions.BaseUrl)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenBaseUrlIsNotAbsolute_ReturnsFailedWithExpectedMessage()
    {
        var options = new ExternalServiceOptions
        {
            BaseUrl = "example.com/api",
            Timeout = TimeSpan.FromSeconds(30)
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeValidHttpOrHttps(null, nameof(ExternalServiceOptions.BaseUrl)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenBaseUrlHasFtpScheme_ReturnsFailedWithExpectedMessage()
    {
        var options = new ExternalServiceOptions
        {
            BaseUrl = "ftp://example.com",
            Timeout = TimeSpan.FromSeconds(30)
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeValidHttpOrHttps(null, nameof(ExternalServiceOptions.BaseUrl)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenTimeoutIsZero_ReturnsFailedWithExpectedMessage()
    {
        var options = new ExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            Timeout = TimeSpan.Zero
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeGreaterThanZero(null, nameof(ExternalServiceOptions.Timeout)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenTimeoutIsNegative_ReturnsFailedWithExpectedMessage()
    {
        var options = new ExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            Timeout = TimeSpan.FromSeconds(-1)
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeGreaterThanZero(null, nameof(ExternalServiceOptions.Timeout)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenHttpUrlWithPositiveTimeout_ReturnsSuccess()
    {
        var options = new ExternalServiceOptions
        {
            BaseUrl = "http://example.com",
            Timeout = TimeSpan.FromSeconds(30)
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_WhenHttpsUrlWithPositiveTimeout_ReturnsSuccess()
    {
        var options = new ExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            Timeout = TimeSpan.FromSeconds(60)
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }
}
