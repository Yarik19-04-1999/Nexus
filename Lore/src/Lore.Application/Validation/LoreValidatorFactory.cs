using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.ValidationContexts;
using Microsoft.Extensions.DependencyInjection;

namespace Lore.Application.Validation;

public class LoreValidatorFactory : ILoreValidatorFactory
{
    private static readonly ObjectFactory _createMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(CreateMovieValidator), []);
    private static readonly ObjectFactory _createUniverseFactory =
        ActivatorUtilities.CreateFactory(typeof(CreateUniverseValidator), []);
    private static readonly ObjectFactory _updateMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(UpdateMovieValidator), [typeof(UpdateMovieValidationContext)]);
    private static readonly ObjectFactory _updateUniverseFactory =
        ActivatorUtilities.CreateFactory(typeof(UpdateUniverseValidator), [typeof(UpdateUniverseValidationContext)]);
    private static readonly ObjectFactory _deleteMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(DeleteMovieValidator), [typeof(MovieValidationContext)]);
    private static readonly ObjectFactory _deleteUniverseFactory =
        ActivatorUtilities.CreateFactory(typeof(DeleteUniverseValidator), [typeof(UniverseValidationContext)]);
    private static readonly ObjectFactory _getMovieByIdFactory =
        ActivatorUtilities.CreateFactory(typeof(GetMovieByIdValidator), [typeof(MovieValidationContext)]);
    private static readonly ObjectFactory _getUniverseByIdFactory =
        ActivatorUtilities.CreateFactory(typeof(GetUniverseByIdValidator), [typeof(UniverseValidationContext)]);
    private static readonly ObjectFactory _incrementViewCountFactory =
        ActivatorUtilities.CreateFactory(typeof(IncrementMovieViewCountValidator), [typeof(MovieValidationContext)]);
    private static readonly ObjectFactory _decrementViewCountFactory =
        ActivatorUtilities.CreateFactory(typeof(DecrementMovieViewCountValidator), [typeof(MovieValidationContext)]);
    private static readonly ObjectFactory _linkMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(LinkMovieToUniverseValidator), [typeof(LinkMovieToUniverseValidationContext)]);
    private static readonly ObjectFactory _unlinkMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(UnlinkMovieFromUniverseValidator), [typeof(MovieValidationContext)]);

    private readonly IServiceProvider _sp;

    public LoreValidatorFactory(IServiceProvider sp) => _sp = sp;

    public ICreateMovieValidator CreateMovieValidator()
        => (ICreateMovieValidator)_createMovieFactory(_sp, null);
    public ICreateUniverseValidator CreateUniverseValidator()
        => (ICreateUniverseValidator)_createUniverseFactory(_sp, null);
    public IUpdateMovieValidator UpdateMovieValidator(UpdateMovieValidationContext context)
        => (IUpdateMovieValidator)_updateMovieFactory(_sp, [context]);
    public IUpdateUniverseValidator UpdateUniverseValidator(UpdateUniverseValidationContext context)
        => (IUpdateUniverseValidator)_updateUniverseFactory(_sp, [context]);
    public IDeleteMovieValidator DeleteMovieValidator(MovieValidationContext context)
        => (IDeleteMovieValidator)_deleteMovieFactory(_sp, [context]);
    public IDeleteUniverseValidator DeleteUniverseValidator(UniverseValidationContext context)
        => (IDeleteUniverseValidator)_deleteUniverseFactory(_sp, [context]);
    public IGetMovieByIdValidator GetMovieByIdValidator(MovieValidationContext context)
        => (IGetMovieByIdValidator)_getMovieByIdFactory(_sp, [context]);
    public IGetUniverseByIdValidator GetUniverseByIdValidator(UniverseValidationContext context)
        => (IGetUniverseByIdValidator)_getUniverseByIdFactory(_sp, [context]);
    public IIncrementMovieViewCountValidator IncrementMovieViewCountValidator(MovieValidationContext context)
        => (IIncrementMovieViewCountValidator)_incrementViewCountFactory(_sp, [context]);
    public IDecrementMovieViewCountValidator DecrementMovieViewCountValidator(MovieValidationContext context)
        => (IDecrementMovieViewCountValidator)_decrementViewCountFactory(_sp, [context]);
    public ILinkMovieToUniverseValidator LinkMovieToUniverseValidator(LinkMovieToUniverseValidationContext context)
        => (ILinkMovieToUniverseValidator)_linkMovieFactory(_sp, [context]);
    public IUnlinkMovieFromUniverseValidator UnlinkMovieFromUniverseValidator(MovieValidationContext context)
        => (IUnlinkMovieFromUniverseValidator)_unlinkMovieFactory(_sp, [context]);
}
