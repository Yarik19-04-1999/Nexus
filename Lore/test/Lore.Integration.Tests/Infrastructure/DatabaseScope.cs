using Lore.Application.Models;
using Lore.Infrastructure.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace Lore.Integration.Tests.Infrastructure;

public sealed class DatabaseScope : IAsyncDisposable
{
    private readonly IServiceScope _scope;

    public LoreDbContext Context { get; }

    public DatabaseScope(LoreWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Context = _scope.ServiceProvider.GetRequiredService<LoreDbContext>();
    }

    public async Task<Universe> SeedUniverse(Action<Universe>? configure = null)
    {
        var universe = new Universe
        {
            Name = $"Test-{Guid.NewGuid()}",
        };
        configure?.Invoke(universe);
        Context.Universes.Add(universe);
        await Context.SaveChangesAsync();
        return universe;
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        _scope.Dispose();
    }
}
