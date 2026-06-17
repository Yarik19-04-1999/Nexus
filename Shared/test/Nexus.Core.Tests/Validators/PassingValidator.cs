using Microsoft.Extensions.Options;
using Nexus.Core.Tests.Models;

namespace Nexus.Core.Tests.Validators;

public class PassingValidator : IValidateOptions<DummyOptions>
{
    public ValidateOptionsResult Validate(string? name, DummyOptions options)
        => ValidateOptionsResult.Success;
}
