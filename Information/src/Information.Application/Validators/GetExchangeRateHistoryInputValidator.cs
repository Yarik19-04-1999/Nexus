using FluentValidation;
using Information.Application.Models.Input;

namespace Information.Application.Validators;

public class GetExchangeRateHistoryInputValidator : AbstractValidator<GetExchangeRateHistoryInput>
{
    public GetExchangeRateHistoryInputValidator()
    {
        RuleFor(x => x.Currency)
            .IsInEnum()
            .When(x => x.Currency.HasValue);
    }
}
