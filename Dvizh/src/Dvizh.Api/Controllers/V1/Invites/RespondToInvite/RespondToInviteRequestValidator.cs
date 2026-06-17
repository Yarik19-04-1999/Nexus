using Dvizh.Application.Constants;
using FluentValidation;

namespace Dvizh.Api.Controllers.V1.Invites.RespondToInvite;

public class RespondToInviteRequestValidator : AbstractValidator<RespondToInviteRequest>
{
    public RespondToInviteRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(InviteValidationConstants.CodeMaxLength);

        RuleFor(x => x.Answer)
            .IsInEnum()
            .NotEqual(Dvizh.Application.Enums.InviteAnswer.Pending)
            .WithMessage("Answer must be Yes or No.");
    }
}
