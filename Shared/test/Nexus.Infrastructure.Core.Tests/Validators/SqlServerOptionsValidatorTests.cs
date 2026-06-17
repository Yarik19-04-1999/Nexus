using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;

namespace Nexus.Infrastructure.Core.Tests.Validators;

public class SqlServerOptionsValidatorTests
{
    private readonly SqlServerOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenConnectionStringIsNull_ReturnsFailed()
    {
        var options = new SqlServerOptions { ConnectionString = null! };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_WhenConnectionStringIsEmpty_ReturnsFailed()
    {
        var options = new SqlServerOptions { ConnectionString = string.Empty };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_WhenConnectionStringIsWhitespace_ReturnsFailed()
    {
        var options = new SqlServerOptions { ConnectionString = "   " };

        var result = _validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_WhenConnectionStringIsValid_ReturnsSuccess()
    {
        var options = new SqlServerOptions { ConnectionString = "Server=.;Database=Test;" };

        var result = _validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }
}
