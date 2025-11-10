using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence.Configurations;

public class OrderItemOptionConfiguration : IEntityTypeConfiguration<OrderItemOption>
{
    public void Configure(EntityTypeBuilder<OrderItemOption> builder)
    {
        builder.ToTable("OrderItemOptions");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OptionName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.ExtraPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.Quantity)
            .IsRequired();

        builder.HasIndex(o => o.OrderItemId);
        builder.HasIndex(o => o.MenuOptionId);
    }
}

