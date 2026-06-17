using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Nexus.Core.Integration.Tests.Extensions;

public static class ServiceCollectionTestExtensions
{
    public static IServiceCollection ReplaceWith<TService>(
        this IServiceCollection services,
        TService instance) where TService : class
    {
        services.RemoveAll<TService>();
        services.AddSingleton(instance);
        return services;
    }

    public static Mock<TService> ReplaceWithMock<TService>(
        this IServiceCollection services,
        Action<Mock<TService>>? setup = null) where TService : class
    {
        var mock = new Mock<TService>();
        setup?.Invoke(mock);
        services.ReplaceWith(mock.Object);
        return mock;
    }
}
