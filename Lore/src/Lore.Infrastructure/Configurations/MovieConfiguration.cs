using Lore.Application.Constants;
using Lore.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Infrastructure.Core.Constants;

namespace Lore.Infrastructure.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);

        builder.Property(x => x.Title).HasMaxLength(MovieValidationConstants.TitleMaxLength);
        builder.Property(x => x.ReviewText).HasColumnType(SqlServerDefaultConstants.NVarCharMax);
        builder.Property(x => x.Score).HasColumnType("decimal(3,1)");
        builder.Property(x => x.RewatchStatus).HasConversion<int>();

        builder.HasOne(x => x.Universe)
            .WithMany(x => x.Movies)
            .HasForeignKey(x => x.UniverseId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
