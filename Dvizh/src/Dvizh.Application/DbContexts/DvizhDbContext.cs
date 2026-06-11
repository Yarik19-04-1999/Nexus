using Dvizh.Application.Constants;
using Dvizh.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Dvizh.Application.DbContexts;

public class DvizhDbContext : DbContext
{
    public DvizhDbContext(DbContextOptions<DvizhDbContext> options) : base(options) { }

    public DbSet<Invite> Invites => Set<Invite>();
    public DbSet<InviteEvent> InviteEvents => Set<InviteEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SqlConstants.Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DvizhDbContext).Assembly);
    }
}
