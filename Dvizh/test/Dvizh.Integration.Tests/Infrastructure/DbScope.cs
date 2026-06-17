using Dvizh.Application.DbContexts;
using Dvizh.Application.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.Infrastructure;

/// <summary>
/// Opens a scoped DvizhDbContext for a test. Data created during tests is left in the DB intentionally.
/// </summary>
public sealed class DbScope : IAsyncDisposable
{
    private readonly IServiceScope _scope;

    public DvizhDbContext Db { get; }

    public DbScope(DvizhWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Db = _scope.ServiceProvider.GetRequiredService<DvizhDbContext>();
    }

    /// <summary>Seeds a minimal invite directly into the DB.</summary>
    public async Task<Invite> SeedInvite(Action<Invite>? configure = null)
    {
        var invite = new Invite
        {
            Code = Guid.NewGuid().ToString("N")[..10],
            Message = $"Test-{Guid.NewGuid()}",
        };
        configure?.Invoke(invite);
        Db.Invites.Add(invite);
        await Db.SaveChangesAsync();
        return invite;
    }

    public async ValueTask DisposeAsync()
    {
        await Db.DisposeAsync();
        _scope.Dispose();
    }
}
