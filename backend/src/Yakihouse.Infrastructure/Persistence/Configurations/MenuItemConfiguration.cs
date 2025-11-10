using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Description)
            .HasMaxLength(1000);

        builder.Property(i => i.BasePrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasOne(i => i.KitchenStation)
            .WithMany()
            .HasForeignKey(i => i.KitchenStationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(i => i.OptionGroups)
            .WithOne(g => g.MenuItem)
            .HasForeignKey(g => g.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.Name);
        builder.HasIndex(i => i.CategoryId);
        builder.HasIndex(i => i.KitchenStationId);
    }
}

