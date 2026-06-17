using FluentValidation;
using Information.Application.Models.Input;

namespace Information.Application.Validators;

public class SetUserLanguageInputValidator : AbstractValidator<SetUserLanguageInput>
{
    public SetUserLanguageInputValidator()
    {
        RuleFor(x => x.TelegramUserId)
            .GreaterThan(0);

        RuleFor(x => x.Language)
            .IsInEnum();
    }
}
