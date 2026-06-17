using FluentValidation;
using Information.Application.Models.Input;

namespace Information.Application.Validators;

public class GetUserLanguageInputValidator : AbstractValidator<GetUserLanguageInput>
{
    public GetUserLanguageInputValidator()
    {
        RuleFor(x => x.TelegramUserId)
            .GreaterThan(0);
    }
}
