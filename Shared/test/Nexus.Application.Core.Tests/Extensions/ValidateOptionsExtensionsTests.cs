using Microsoft.Extensions.Options;
using Nexus.Application.Core.Extensions;
using Nexus.Core.Tests.Constants;
using Nexus.Core.Tests.Models;
using Nexus.Core.Tests.Validators;

namespace Nexus.Application.Core.Tests.Extensions;

public class ValidateOptionsExtensionsTests
{

    [Fact]
    public void ValidateAndThrow_WhenValidatorPasses_DoesNotThrow()
    {
        var validator = new PassingValidator();
        var options = new DummyOptions();

        validator.ValidateAndThrow(options);
    }

    [Fact]
    public void ValidateAndThrow_WhenValidatorFails_ThrowsOptionsValidationException()
    {
        var validator = new FailingValidator();
        var options = new DummyOptions();

        var ex = Assert.Throws<OptionsValidationException>(() => validator.ValidateAndThrow(options));

        Assert.Single(ex.Failures);

        var failure = ex.Failures.First();
        Assert.Contains(TestErrorMessages.Default, failure);
    }
}
