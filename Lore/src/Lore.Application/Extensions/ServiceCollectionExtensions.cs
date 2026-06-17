using Lore.Application.Interfaces.UseCases;
using Lore.Application.UseCases;
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
            .AddScoped<IDeleteUniverseUseCase, DeleteUniverseUseCase>();
}
