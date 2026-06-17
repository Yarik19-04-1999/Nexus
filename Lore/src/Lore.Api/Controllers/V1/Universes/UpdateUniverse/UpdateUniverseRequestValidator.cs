using FluentValidation;
using Lore.Application.Constants;

namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

public class UpdateUniverseRequestValidator : AbstractValidator<UpdateUniverseRequest>
{
    public UpdateUniverseRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(UniverseValidationConstants.NameMaxLength);

        RuleFor(x => x.ListNo)
            .GreaterThanOrEqualTo(0);
    }
}
