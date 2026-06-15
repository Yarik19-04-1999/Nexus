using Information.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Information.Infrastructure.DbContexts;

public class InformationDbContext : DbContext
{
    public InformationDbContext(DbContextOptions<InformationDbContext> options) : base(options) { }

    public DbSet<TelegramUser> TelegramUsers => Set<TelegramUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("IceAgeBrief");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InformationDbContext).Assembly);
    }
}
