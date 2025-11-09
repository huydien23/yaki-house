using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class MenuOptionGroupConfiguration : IEntityTypeConfiguration<MenuOptionGroup>
{
    public void Configure(EntityTypeBuilder<MenuOptionGroup> builder)
    {
        builder.ToTable("MenuOptionGroups");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.IsRequired)
            .IsRequired();

        builder.HasIndex(g => g.MenuItemId);
    }
}

