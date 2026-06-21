using Lore.Application.Interfaces.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Lore.Application.Validation;

public class LoreValidatorFactory : ILoreValidatorFactory
{
    private readonly ObjectFactory _createMovieValidatorFactory =
        ActivatorUtilities.CreateFactory(typeof(CreateMovieValidator), []);

    private readonly IServiceProvider _serviceProvider;

    public LoreValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICreateMovieValidator CreateMovieValidator()
        => (ICreateMovieValidator)_createMovieValidatorFactory(_serviceProvider, null);
}
