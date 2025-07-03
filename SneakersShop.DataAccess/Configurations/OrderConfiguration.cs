using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class OrderConfiguration : EntityConfiguration<Order>
{
    public override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.PromisedDate).IsRequired();
            builder.Property(x => x.ReceivedDate).HasDefaultValue(null);
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");

            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.Address)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(x => x.AddressId)
                   .OnDelete(DeleteBehavior.SetNull);
    }
}
