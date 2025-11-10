using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class PayrollEntryConfiguration : IEntityTypeConfiguration<PayrollEntry>
{
    public void Configure(EntityTypeBuilder<PayrollEntry> builder)
    {
        builder.ToTable("PayrollEntries");

        builder.HasKey(pe => pe.Id);

        builder.Property(pe => pe.Period)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pe => pe.GrossPay)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pe => pe.Adjustments)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pe => pe.NetPay)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pe => pe.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(pe => pe.Staff)
            .WithMany()
            .HasForeignKey(pe => pe.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pe => pe.StaffId);
        builder.HasIndex(pe => pe.Period);
        builder.HasIndex(pe => pe.Status);
    }
}

