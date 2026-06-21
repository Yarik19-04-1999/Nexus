using FluentValidation;

namespace Nexus.Application.Core.Validation;

public abstract class ValidatorBase<T> : AbstractValidator<T>
{
    protected ValidatorBase()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
    }
}
