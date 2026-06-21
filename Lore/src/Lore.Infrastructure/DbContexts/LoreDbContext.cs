using Lore.Application.Constants;
using Lore.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Lore.Infrastructure.DbContexts;

public class LoreDbContext : DbContext
{
    public LoreDbContext(DbContextOptions<LoreDbContext> options) : base(options) { }

    public DbSet<Universe> Universes => Set<Universe>();
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SqlConstants.Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoreDbContext).Assembly);
    }
}
