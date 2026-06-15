using Information.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Information.Infrastructure.DbContexts.Configurations;

public class TelegramUserConfiguration : IEntityTypeConfiguration<TelegramUser>
{
    public void Configure(EntityTypeBuilder<TelegramUser> builder)
    {
        builder.ToTable("TelegramUsers");
        builder.HasKey(x => x.TelegramUserId);
        builder.Property(x => x.Language).HasConversion<int>();
    }
}
