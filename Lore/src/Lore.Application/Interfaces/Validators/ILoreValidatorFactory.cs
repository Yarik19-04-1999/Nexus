using Lore.Application.Models.ValidationContexts;

namespace Lore.Application.Interfaces.Validators;

public interface ILoreValidatorFactory
{
    ICreateMovieValidator CreateMovieValidator();
    ICreateUniverseValidator CreateUniverseValidator();

    IUpdateMovieValidator UpdateMovieValidator(UpdateMovieValidationContext context);
    IUpdateUniverseValidator UpdateUniverseValidator(UpdateUniverseValidationContext context);

    IDeleteMovieValidator DeleteMovieValidator(MovieValidationContext context);
    IDeleteUniverseValidator DeleteUniverseValidator(UniverseValidationContext context);

    IGetMovieByIdValidator GetMovieByIdValidator(MovieValidationContext context);
    IGetUniverseByIdValidator GetUniverseByIdValidator(UniverseValidationContext context);

    IIncrementMovieViewCountValidator IncrementMovieViewCountValidator(MovieValidationContext context);
    IDecrementMovieViewCountValidator DecrementMovieViewCountValidator(MovieValidationContext context);

    ILinkMovieToUniverseValidator LinkMovieToUniverseValidator(LinkMovieToUniverseValidationContext context);
    IUnlinkMovieFromUniverseValidator UnlinkMovieFromUniverseValidator(MovieValidationContext context);
}
