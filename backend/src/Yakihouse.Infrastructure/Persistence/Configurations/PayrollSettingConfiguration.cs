using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class PayrollSettingConfiguration : IEntityTypeConfiguration<PayrollSetting>
{
    public void Configure(EntityTypeBuilder<PayrollSetting> builder)
    {
        builder.ToTable("PayrollSettings");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.BaseRate)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(ps => ps.Allowance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(ps => ps.Role)
            .WithMany()
            .HasForeignKey(ps => ps.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ps => ps.RoleId)
            .IsUnique();
    }
}

