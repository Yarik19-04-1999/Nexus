using Nexus.Application.Core.Constants;
using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;

namespace Nexus.Infrastructure.Core.Tests.Validators;

public class ApiKeyExternalServiceOptionsValidatorTests
{
    private readonly ApiKeyExternalServiceOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenBaseUrlIsInvalid_ReturnsBaseFailureMessage()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = string.Empty,
            ApiKey = "valid-key"
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeNotEmpty(null, nameof(ApiKeyExternalServiceOptions.BaseUrl)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenApiKeyIsEmpty_ReturnsFailedWithExpectedMessage()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            Timeout = TimeSpan.FromSeconds(30),
            ApiKey = string.Empty
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeNotEmpty(null, nameof(ApiKeyExternalServiceOptions.ApiKey)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenApiKeyIsWhitespace_ReturnsFailedWithExpectedMessage()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            Timeout = TimeSpan.FromSeconds(30),
            ApiKey = "   "
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
        Assert.Equal(
            OptionsErrorMessages.MustBeNotEmpty(null, nameof(ApiKeyExternalServiceOptions.ApiKey)),
            result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenAllValid_ReturnsSuccess()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            Timeout = TimeSpan.FromSeconds(30),
            ApiKey = "my-api-key"
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }
}
