using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class BillPromotionConfiguration : IEntityTypeConfiguration<BillPromotion>
{
    public void Configure(EntityTypeBuilder<BillPromotion> builder)
    {
        builder.ToTable("BillPromotions");

        builder.HasKey(bp => bp.Id);

        builder.Property(bp => bp.AppliedValue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(bp => bp.Promotion)
            .WithMany()
            .HasForeignKey(bp => bp.PromotionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(bp => bp.BillId);
        builder.HasIndex(bp => bp.PromotionId);
    }
}

