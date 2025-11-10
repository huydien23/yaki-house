using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class DiningTableConfiguration : IEntityTypeConfiguration<DiningTable>
{
    public void Configure(EntityTypeBuilder<DiningTable> builder)
    {
        builder.ToTable("DiningTables");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Zone)
            .HasMaxLength(100);

        builder.Property(t => t.Capacity)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasMany(t => t.Assignments)
            .WithOne(a => a.Table)
            .HasForeignKey(a => a.TableId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.Code)
            .IsUnique();
    }
}

