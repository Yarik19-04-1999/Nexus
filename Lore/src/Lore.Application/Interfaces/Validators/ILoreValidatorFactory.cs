using Lore.Application.Models.ValidationContexts;

namespace Lore.Application.Interfaces.Validators;

public interface ILoreValidatorFactory
{
    ICreateMovieValidator CreateCreateMovieValidator();
    ICreateUniverseValidator CreateCreateUniverseValidator();

    IUpdateMovieValidator CreateUpdateMovieValidator(UpdateMovieValidationContext context);
    IUpdateUniverseValidator CreateUpdateUniverseValidator(UpdateUniverseValidationContext context);

    IDeleteMovieValidator CreateDeleteMovieValidator(DeleteMovieValidationContext context);
    IDeleteUniverseValidator CreateDeleteUniverseValidator(DeleteUniverseValidationContext context);

    IGetMovieByIdValidator CreateGetMovieByIdValidator(GetMovieByIdValidationContext context);
    IGetUniverseByIdValidator CreateGetUniverseByIdValidator(GetUniverseByIdValidationContext context);

    IIncrementMovieViewCountValidator CreateIncrementMovieViewCountValidator(IncrementMovieViewCountValidationContext context);
    IDecrementMovieViewCountValidator CreateDecrementMovieViewCountValidator(DecrementMovieViewCountValidationContext context);

    ILinkMovieToUniverseValidator CreateLinkMovieToUniverseValidator(LinkMovieToUniverseValidationContext context);
    IUnlinkMovieFromUniverseValidator CreateUnlinkMovieFromUniverseValidator(UnlinkMovieFromUniverseValidationContext context);
}
