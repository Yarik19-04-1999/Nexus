using Information.Application.Models;
using Information.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;

namespace Information.Infrastructure.DbContexts;

public class InformationDbContext : DbContext
{
    public InformationDbContext(DbContextOptions<InformationDbContext> options) : base(options) { }

    public DbSet<TelegramUser> TelegramUsers => Set<TelegramUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SqlConstants.Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InformationDbContext).Assembly);
    }
}
