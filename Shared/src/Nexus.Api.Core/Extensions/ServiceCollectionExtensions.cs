using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Api.Core.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexusApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning();
        return services;
    }
}
