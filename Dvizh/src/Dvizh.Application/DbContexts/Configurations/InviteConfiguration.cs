using Dvizh.Application.Constants;
using Dvizh.Application.Enums;
using Dvizh.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Infrastructure.Core.Constants;

namespace Dvizh.Application.DbContexts.Configurations;

public class InviteConfiguration : IEntityTypeConfiguration<Invite>
{
    public void Configure(EntityTypeBuilder<Invite> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql(SqlServerDefaultConstants.SysUtcDateTime);

        builder.Property(x => x.Code).HasMaxLength(InviteConstants.CodeMaxLength);
        builder.Property(x => x.Message).HasMaxLength(InviteConstants.MessageMaxLength);
        builder.Property(x => x.Description).HasMaxLength(InviteConstants.DescriptionMaxLength);

        builder.Property(x => x.Answer)
            .HasConversion<int>()
            .HasDefaultValue(InviteAnswer.Pending);

        builder.HasIndex(x => x.Code).IsUnique();
    }
}
