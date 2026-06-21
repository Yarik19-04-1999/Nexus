using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.ValidationContexts;
using Microsoft.Extensions.DependencyInjection;

namespace Lore.Application.Validation;

public class LoreValidatorFactory : ILoreValidatorFactory
{
    private readonly ObjectFactory _createMovieValidatorFactory =
        ActivatorUtilities.CreateFactory(typeof(CreateMovieValidator), []);

    private readonly ObjectFactory _updateMovieValidatorFactory =
        ActivatorUtilities.CreateFactory(typeof(UpdateMovieValidator), [typeof(UpdateMovieValidationContext)]);

    private readonly ObjectFactory _updateUniverseValidatorFactory =
        ActivatorUtilities.CreateFactory(typeof(UpdateUniverseValidator), [typeof(UpdateUniverseValidationContext)]);

    private readonly IServiceProvider _serviceProvider;

    public LoreValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICreateMovieValidator CreateMovieValidator()
        => (ICreateMovieValidator)_createMovieValidatorFactory(_serviceProvider, null);

    public IUpdateMovieValidator UpdateMovieValidator(UpdateMovieValidationContext context)
        => (IUpdateMovieValidator)_updateMovieValidatorFactory(_serviceProvider, [context]);

    public IUpdateUniverseValidator UpdateUniverseValidator(UpdateUniverseValidationContext context)
        => (IUpdateUniverseValidator)_updateUniverseValidatorFactory(_serviceProvider, [context]);
}
