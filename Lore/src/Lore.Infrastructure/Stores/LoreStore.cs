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
        => await this.context.Universes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

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
}
