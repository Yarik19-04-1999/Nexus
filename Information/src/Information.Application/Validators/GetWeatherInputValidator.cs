using FluentValidation;
using Information.Application.Models.Input;

namespace Information.Application.Validators;

public class GetWeatherInputValidator : AbstractValidator<GetWeatherInput>
{
    public GetWeatherInputValidator()
    {
        RuleFor(x => x.City)
            .IsInEnum();
    }
}
