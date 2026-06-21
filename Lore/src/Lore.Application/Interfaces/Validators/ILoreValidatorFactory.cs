namespace Lore.Application.Interfaces.Validators;

public interface ILoreValidatorFactory
{
    ICreateMovieValidator CreateMovieValidator();
}
