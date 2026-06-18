using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Core.Integration.Tests.Extensions;

public static class WebApplicationFactoryExtensions
{
    public static HttpClient CreateClient<TProgram>(
        this WebApplicationFactory<TProgram> factory,
        Action<IServiceCollection> configureServices)
        where TProgram : class =>
        factory.WithWebHostBuilder(b => b.ConfigureServices(configureServices)).CreateClient();

    public static WebApplicationFactory<TProgram> WithConfiguration<TProgram>(
        this WebApplicationFactory<TProgram> factory,
        string key,
        string? value)
        where TProgram : class =>
        factory.WithWebHostBuilder(b => b.ConfigureAppConfiguration((_, config) =>
            config.AddInMemoryCollection(new Dictionary<string, string?> { [key] = value })));

    public static WebApplicationFactory<TProgram> WithConfiguration<TProgram>(
        this WebApplicationFactory<TProgram> factory,
        IReadOnlyDictionary<string, string?> settings)
        where TProgram : class =>
        factory.WithWebHostBuilder(b => b.ConfigureAppConfiguration((_, config) =>
            config.AddInMemoryCollection(settings)));

    public static IServiceScope CreateScope<TProgram>(
        this WebApplicationFactory<TProgram> factory)
        where TProgram : class =>
        factory.Services.CreateScope();

    public static IReadOnlyList<string> GetOpenApiDocumentNames<TProgram>(
        this WebApplicationFactory<TProgram> factory)
        where TProgram : class =>
        factory.Services
            .GetRequiredService<IApiVersionDescriptionProvider>()
            .ApiVersionDescriptions
            .Select(d => d.GroupName)
            .ToArray();
}
