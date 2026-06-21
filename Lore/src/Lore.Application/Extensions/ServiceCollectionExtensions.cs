using Lore.Application.Interfaces.UseCases;
using Lore.Application.UseCases;
using Lore.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Lore.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddScoped<IGetUniversesUseCase, GetUniversesUseCase>()
            .AddScoped<IGetUniverseByIdUseCase, GetUniverseByIdUseCase>()
            .AddScoped<ICreateUniverseUseCase, CreateUniverseUseCase>()
            .AddScoped<IUpdateUniverseUseCase, UpdateUniverseUseCase>()
            .AddScoped<IDeleteUniverseUseCase, DeleteUniverseUseCase>()
            .AddScoped<ISearchUniversesUseCase, SearchUniversesUseCase>()
            .AddScoped<IGetMoviesUseCase, GetMoviesUseCase>()
            .AddScoped<IGetMovieByIdUseCase, GetMovieByIdUseCase>()
            .AddScoped<ICreateMovieUseCase, CreateMovieUseCase>()
            .AddScoped<IUpdateMovieUseCase, UpdateMovieUseCase>()
            .AddScoped<IDeleteMovieUseCase, DeleteMovieUseCase>()
            .AddScoped<ISearchMoviesUseCase, SearchMoviesUseCase>()
            .AddScoped<IIncrementMovieViewCountUseCase, IncrementMovieViewCountUseCase>()
            .AddScoped<IDecrementMovieViewCountUseCase, DecrementMovieViewCountUseCase>()
            .AddScoped<ILinkMovieToUniverseUseCase, LinkMovieToUniverseUseCase>()
            .AddScoped<IUnlinkMovieFromUniverseUseCase, UnlinkMovieFromUniverseUseCase>()
            .AddScoped<ILoreValidatorFactory, LoreValidatorFactory>();
}
