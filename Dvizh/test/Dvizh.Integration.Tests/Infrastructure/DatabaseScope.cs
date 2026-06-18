using Dvizh.Application.DbContexts;
using Dvizh.Application.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.Infrastructure;

public sealed class DatabaseScope : IAsyncDisposable
{
    private readonly IServiceScope _scope;

    public DvizhDbContext Context { get; }

    public DatabaseScope(DvizhWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Context = _scope.ServiceProvider.GetRequiredService<DvizhDbContext>();
    }

    public async Task<Invite> SeedInvite(Action<Invite>? configure = null)
    {
        var invite = new Invite
        {
            Code = Guid.NewGuid().ToString("N")[..10],
            Message = $"Test-{Guid.NewGuid()}",
        };
        configure?.Invoke(invite);
        Context.Invites.Add(invite);
        await Context.SaveChangesAsync();
        return invite;
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        _scope.Dispose();
    }
}
