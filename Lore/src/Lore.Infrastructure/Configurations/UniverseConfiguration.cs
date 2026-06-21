using Lore.Application.Constants;
using Lore.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Infrastructure.Core.Constants;

namespace Lore.Infrastructure.Configurations;

public class UniverseConfiguration : IEntityTypeConfiguration<Universe>
{
    public void Configure(EntityTypeBuilder<Universe> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);

        builder.Property(x => x.Name).HasMaxLength(UniverseValidationConstants.NameMaxLength);
        builder.Property(x => x.Description).HasColumnType(SqlServerDefaultConstants.NVarCharMax);
    }
}
