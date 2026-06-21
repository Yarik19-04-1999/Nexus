using Lore.Application.Models.ValidationContexts;

namespace Lore.Application.Interfaces.Validators;

public interface ILoreValidatorFactory
{
    ICreateMovieValidator CreateMovieValidator();
    IUpdateMovieValidator UpdateMovieValidator(UpdateMovieValidationContext context);
}
