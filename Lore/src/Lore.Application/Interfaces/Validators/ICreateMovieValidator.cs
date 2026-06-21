using FluentValidation;
using Lore.Application.Models.Inputs;

namespace Lore.Application.Interfaces.Validators;

public interface ICreateMovieValidator : IValidator<CreateMovieInput>
{
}
