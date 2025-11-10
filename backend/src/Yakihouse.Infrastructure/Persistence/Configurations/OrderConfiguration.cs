using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(o => o.GuestCount)
            .IsRequired();

        builder.Property(o => o.AdultCount)
            .IsRequired();

        builder.Property(o => o.ChildCount)
            .IsRequired();

        builder.Property(o => o.ChildHeights)
            .HasMaxLength(500); // JSON array string

        builder.Property(o => o.BuffetType)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Nuong");

        builder.Property(o => o.HasDessertBuffet)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.HasOne(o => o.Table)
            .WithMany()
            .HasForeignKey(o => o.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Staff)
            .WithMany()
            .HasForeignKey(o => o.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Audits)
            .WithOne(a => a.Order)
            .HasForeignKey(a => a.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.TableId);
        builder.HasIndex(o => o.StaffId);
        builder.HasIndex(o => o.CreatedAt);
        builder.HasIndex(o => o.Status);
    }
}

