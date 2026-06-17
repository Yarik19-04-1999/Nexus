using Dvizh.Application.DbContexts;
using Dvizh.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.Infrastructure;

/// <summary>
/// Thin helper that opens a scoped DvizhDbContext, seeds an invite for a test,
/// and deletes it (plus any cascade events) on dispose.
/// </summary>
public sealed class DbScope : IAsyncDisposable
{
    private readonly IServiceScope _scope;
    private readonly List<int> _inviteIds = [];

    public DvizhDbContext Db { get; }

    public DbScope(DvizhWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Db = _scope.ServiceProvider.GetRequiredService<DvizhDbContext>();
    }

    /// <summary>Seeds a minimal invite directly into the DB and tracks it for cleanup.</summary>
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
        Track(invite.Id);
        return invite;
    }

    /// <summary>Tracks an invite ID for cleanup even if it was created outside this scope.</summary>
    public void Track(int inviteId) => _inviteIds.Add(inviteId);

    public async ValueTask DisposeAsync()
    {
        if (_inviteIds.Count > 0)
        {
            await Db.Invites
                .Where(i => _inviteIds.Contains(i.Id))
                .ExecuteDeleteAsync();
        }

        await Db.DisposeAsync();
        _scope.Dispose();
    }
}
