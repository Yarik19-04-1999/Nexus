using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;

namespace Nexus.Infrastructure.Core.Tests.Validators;

public class ApiKeyExternalServiceOptionsValidatorTests
{
    private readonly ApiKeyExternalServiceOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenBaseUrlIsInvalid_ReturnsFailed()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = string.Empty,
            ApiKey = "valid-key"
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_WhenApiKeyIsEmpty_ReturnsFailed()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            ApiKey = string.Empty
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_WhenApiKeyIsWhitespace_ReturnsFailed()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            ApiKey = "   "
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_WhenValidBaseAndValidApiKey_ReturnsSuccess()
    {
        var options = new ApiKeyExternalServiceOptions
        {
            BaseUrl = "https://example.com",
            ApiKey = "my-api-key",
            Timeout = TimeSpan.FromSeconds(30)
        };

        var result = _validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }
}
