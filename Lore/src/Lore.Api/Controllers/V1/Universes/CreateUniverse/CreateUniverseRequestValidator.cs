using FluentValidation;
using Lore.Application.Constants;

namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

public class CreateUniverseRequestValidator : AbstractValidator<CreateUniverseRequest>
{
    public CreateUniverseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(UniverseValidationConstants.NameMaxLength);

        RuleFor(x => x.ListNo)
            .GreaterThanOrEqualTo(0);
    }
}
