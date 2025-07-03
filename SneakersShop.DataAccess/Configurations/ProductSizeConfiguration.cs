using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class ProductSizeConfiguration : EntityConfiguration<ProductSize>
{
    public override void ConfigureEntity(EntityTypeBuilder<ProductSize> builder)
    {
        builder.Property(x => x.Quantity).IsRequired();

            builder.HasOne(x => x.ProductColor)
                .WithMany(x => x.ProductSizes)
                .HasForeignKey(x => x.ProductColorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Size)
                .WithMany(x => x.SizeProducts)
                .HasForeignKey(x => x.SizeId)
                .OnDelete(DeleteBehavior.Cascade);

    }
}
