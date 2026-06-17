using Microsoft.Extensions.Options;
using Nexus.Core.Tests.Constants;
using Nexus.Core.Tests.Models;

namespace Nexus.Core.Tests.Validators;

public class FailingValidator : IValidateOptions<DummyOptions>
{
    public ValidateOptionsResult Validate(string? name, DummyOptions options)
        => ValidateOptionsResult.Fail(TestErrorMessages.Default);
}
