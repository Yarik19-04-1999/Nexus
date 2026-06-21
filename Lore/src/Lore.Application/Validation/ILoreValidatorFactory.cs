namespace Lore.Application.Validation;

public interface ILoreValidatorFactory
{
    ICreateMovieValidator CreateMovieValidator(CreateMovieValidationContext context);
}
