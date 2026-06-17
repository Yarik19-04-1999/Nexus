using Dvizh.Application.Constants;
using FluentValidation;

namespace Dvizh.Api.Controllers.V1.Invites.CreateInvite;

public class CreateInviteRequestValidator : AbstractValidator<CreateInviteRequest>
{
    public CreateInviteRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(InviteValidationConstants.MessageMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(InviteValidationConstants.DescriptionMaxLength);

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(_ => DateTime.UtcNow)
            .When(x => x.ExpiresAt.HasValue);

        RuleFor(x => x.Language)
            .IsInEnum();

        RuleFor(x => x.Mascot)
            .IsInEnum();
    }
}
