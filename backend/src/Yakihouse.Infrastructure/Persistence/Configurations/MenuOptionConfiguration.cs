using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class MenuOptionConfiguration : IEntityTypeConfiguration<MenuOption>
{
    public void Configure(EntityTypeBuilder<MenuOption> builder)
    {
        builder.ToTable("MenuOptions");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.ExtraPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.IsDefault)
            .IsRequired();

        builder.HasOne(o => o.Group)
            .WithMany(g => g.Options)
            .HasForeignKey(o => o.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.GroupId);
    }
}

