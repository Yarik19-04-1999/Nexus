using Lore.Application.Interfaces.Stores;
using Lore.Application.Models;
using Lore.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Models;
using Sieve.Models;
using Sieve.Services;

namespace Lore.Infrastructure.Stores;

public class LoreStore : ILoreStore
{
    private readonly LoreDbContext context;
    private readonly ISieveProcessor sieve;

    public LoreStore(LoreDbContext context, ISieveProcessor sieve)
    {
        this.context = context;
        this.sieve = sieve;
    }

    public async Task<PagedResult<Universe>> GetUniversesPaged(SieveModel model, CancellationToken cancellationToken)
    {
        var query = this.context.Universes.AsNoTracking();
        var total = await this.sieve.Apply(model, query, applyPagination: false).CountAsync(cancellationToken);
        var items = await this.sieve.Apply(model, query).ToListAsync(cancellationToken);
        return new PagedResult<Universe>(items, total, model.Page ?? 1, model.PageSize ?? 20);
    }

    public async Task<Universe?> GetUniverseById(int id, CancellationToken cancellationToken)
        => await this.context.Universes
            .Include(u => u.Movies)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task CreateUniverse(Universe universe, CancellationToken cancellationToken)
    {
        this.context.Universes.Add(universe);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUniverse(Universe universe, CancellationToken cancellationToken)
    {
        this.context.Universes.Update(universe);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUniverse(Universe universe, CancellationToken cancellationToken)
    {
        this.context.Universes.Remove(universe);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Universe>> SearchUniverses(string query, CancellationToken cancellationToken)
        => await this.context.Universes
            .AsNoTracking()
            .Where(x => x.Name.StartsWith(query))
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    public async Task<PagedResult<Movie>> GetMoviesPaged(SieveModel model, CancellationToken cancellationToken)
    {
        var query = this.context.Movies.AsNoTracking().Include(m => m.Universe);
        var total = await this.sieve.Apply(model, query, applyPagination: false).CountAsync(cancellationToken);
        var items = await this.sieve.Apply(model, query).ToListAsync(cancellationToken);
        return new PagedResult<Movie>(items, total, model.Page ?? 1, model.PageSize ?? 20);
    }

    public async Task<Movie?> GetMovieById(int id, CancellationToken cancellationToken)
        => await this.context.Movies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> MovieExistsByTitleAndYear(string title, int releaseYear, CancellationToken cancellationToken)
        => await this.context.Movies
            .AsNoTracking()
            .AnyAsync(x => x.Title == title && x.ReleaseYear == releaseYear, cancellationToken);

    public async Task CreateMovie(Movie movie, CancellationToken cancellationToken)
    {
        this.context.Movies.Add(movie);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateMovie(Movie movie, CancellationToken cancellationToken)
    {
        this.context.Movies.Update(movie);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMovie(Movie movie, CancellationToken cancellationToken)
    {
        this.context.Movies.Remove(movie);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Movie>> SearchMovies(string query, CancellationToken cancellationToken)
        => await this.context.Movies
            .AsNoTracking()
            .Where(x => x.Title.StartsWith(query))
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);
}
