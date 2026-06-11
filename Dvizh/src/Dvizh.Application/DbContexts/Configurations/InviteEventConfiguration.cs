using Dvizh.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Infrastructure.Core.Constants;

namespace Dvizh.Application.DbContexts.Configurations;

public class InviteEventConfiguration : IEntityTypeConfiguration<InviteEvent>
{
    public void Configure(EntityTypeBuilder<InviteEvent> builder)
    {
        builder.HasKey(x => x.Id).IsClustered(false);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);
        builder.Property(x => x.EventType).HasConversion<int>();

        builder.HasOne(x => x.Invite)
            .WithMany()
            .HasForeignKey(x => x.InviteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.InviteId, x.CreatedAt }).IsClustered();
    }
}
