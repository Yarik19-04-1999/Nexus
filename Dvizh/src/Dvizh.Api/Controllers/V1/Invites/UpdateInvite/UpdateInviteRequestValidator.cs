using Dvizh.Application.Constants;
using FluentValidation;

namespace Dvizh.Api.Controllers.V1.Invites.UpdateInvite;

public class UpdateInviteRequestValidator : AbstractValidator<UpdateInviteRequest>
{
    public UpdateInviteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

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
