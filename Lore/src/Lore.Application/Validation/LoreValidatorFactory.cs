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
        ActivatorUtilities.CreateFactory(typeof(DeleteMovieValidator), [typeof(DeleteMovieValidationContext)]);
    private static readonly ObjectFactory _deleteUniverseFactory =
        ActivatorUtilities.CreateFactory(typeof(DeleteUniverseValidator), [typeof(DeleteUniverseValidationContext)]);
    private static readonly ObjectFactory _getMovieByIdFactory =
        ActivatorUtilities.CreateFactory(typeof(GetMovieByIdValidator), [typeof(GetMovieByIdValidationContext)]);
    private static readonly ObjectFactory _getUniverseByIdFactory =
        ActivatorUtilities.CreateFactory(typeof(GetUniverseByIdValidator), [typeof(GetUniverseByIdValidationContext)]);
    private static readonly ObjectFactory _incrementViewCountFactory =
        ActivatorUtilities.CreateFactory(typeof(IncrementMovieViewCountValidator), [typeof(IncrementMovieViewCountValidationContext)]);
    private static readonly ObjectFactory _decrementViewCountFactory =
        ActivatorUtilities.CreateFactory(typeof(DecrementMovieViewCountValidator), [typeof(DecrementMovieViewCountValidationContext)]);
    private static readonly ObjectFactory _linkMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(LinkMovieToUniverseValidator), [typeof(LinkMovieToUniverseValidationContext)]);
    private static readonly ObjectFactory _unlinkMovieFactory =
        ActivatorUtilities.CreateFactory(typeof(UnlinkMovieFromUniverseValidator), [typeof(UnlinkMovieFromUniverseValidationContext)]);

    private readonly IServiceProvider _sp;

    public LoreValidatorFactory(IServiceProvider sp) => _sp = sp;

    public ICreateMovieValidator CreateCreateMovieValidator()
        => (ICreateMovieValidator)_createMovieFactory(_sp, null);
    public ICreateUniverseValidator CreateCreateUniverseValidator()
        => (ICreateUniverseValidator)_createUniverseFactory(_sp, null);
    public IUpdateMovieValidator CreateUpdateMovieValidator(UpdateMovieValidationContext context)
        => (IUpdateMovieValidator)_updateMovieFactory(_sp, [context]);
    public IUpdateUniverseValidator CreateUpdateUniverseValidator(UpdateUniverseValidationContext context)
        => (IUpdateUniverseValidator)_updateUniverseFactory(_sp, [context]);
    public IDeleteMovieValidator CreateDeleteMovieValidator(DeleteMovieValidationContext context)
        => (IDeleteMovieValidator)_deleteMovieFactory(_sp, [context]);
    public IDeleteUniverseValidator CreateDeleteUniverseValidator(DeleteUniverseValidationContext context)
        => (IDeleteUniverseValidator)_deleteUniverseFactory(_sp, [context]);
    public IGetMovieByIdValidator CreateGetMovieByIdValidator(GetMovieByIdValidationContext context)
        => (IGetMovieByIdValidator)_getMovieByIdFactory(_sp, [context]);
    public IGetUniverseByIdValidator CreateGetUniverseByIdValidator(GetUniverseByIdValidationContext context)
        => (IGetUniverseByIdValidator)_getUniverseByIdFactory(_sp, [context]);
    public IIncrementMovieViewCountValidator CreateIncrementMovieViewCountValidator(IncrementMovieViewCountValidationContext context)
        => (IIncrementMovieViewCountValidator)_incrementViewCountFactory(_sp, [context]);
    public IDecrementMovieViewCountValidator CreateDecrementMovieViewCountValidator(DecrementMovieViewCountValidationContext context)
        => (IDecrementMovieViewCountValidator)_decrementViewCountFactory(_sp, [context]);
    public ILinkMovieToUniverseValidator CreateLinkMovieToUniverseValidator(LinkMovieToUniverseValidationContext context)
        => (ILinkMovieToUniverseValidator)_linkMovieFactory(_sp, [context]);
    public IUnlinkMovieFromUniverseValidator CreateUnlinkMovieFromUniverseValidator(UnlinkMovieFromUniverseValidationContext context)
        => (IUnlinkMovieFromUniverseValidator)_unlinkMovieFactory(_sp, [context]);
}
