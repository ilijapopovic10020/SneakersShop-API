using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class OrderItemConfiguration : EntityConfiguration<OrderItem>
{
    public override void ConfigureEntity(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.ProductSize)
               .WithMany(x => x.OrderItems)
               .HasForeignKey(x => x.ProductSizeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
