using FluentValidation;
using Lore.Application.Models.Inputs;

namespace Lore.Application.Validation;

public interface ICreateMovieValidator : IValidator<CreateMovieInput>
{
}
