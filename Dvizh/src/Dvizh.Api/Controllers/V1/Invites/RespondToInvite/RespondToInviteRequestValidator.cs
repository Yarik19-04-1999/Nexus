using Dvizh.Application.Constants;
using Dvizh.Application.Enums;
using FluentValidation;

namespace Dvizh.Api.Controllers.V1.Invites.RespondToInvite;

public class RespondToInviteRequestValidator : AbstractValidator<RespondToInviteRequest>
{
    public RespondToInviteRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(InviteValidationConstants.Invite.CodeMaxLength);

        RuleFor(x => x.Answer)
            .IsInEnum()
            .NotEqual(InviteAnswer.Pending)
            .WithMessage("Answer must be Yes or No.");
    }
}
