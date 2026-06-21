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
            Name = $"Test universe {Guid.NewGuid()}",
        };
        configure?.Invoke(universe);
        Context.Universes.Add(universe);
        await Context.SaveChangesAsync();
        return universe;
    }

    public async Task<Movie> SeedMovie(Action<Movie>? configure = null)
    {
        var movie = new Movie
        {
            Title = $"Test movie {Guid.NewGuid()}",
            ReleaseYear = 2024,
            DurationMinutes = 120,
        };
        configure?.Invoke(movie);
        Context.Movies.Add(movie);
        await Context.SaveChangesAsync();
        return movie;
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        _scope.Dispose();
    }
}
