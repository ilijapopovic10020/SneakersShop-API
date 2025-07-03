using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class DiscountConfiguration : EntityConfiguration<Discount>
{
    public override void ConfigureEntity(EntityTypeBuilder<Discount> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Percentage).IsRequired().HasPrecision(5, 2);

        builder.HasMany(x => x.ProductDiscounts)
            .WithOne(x => x.Discount)
            .HasForeignKey(x => x.DiscountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
