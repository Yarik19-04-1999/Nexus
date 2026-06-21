using FluentValidation;
using Lore.Application.Models.Inputs;

namespace Lore.Application.Interfaces.Validators;

public interface IUpdateMovieValidator : IValidator<UpdateMovieInput>
{
}
