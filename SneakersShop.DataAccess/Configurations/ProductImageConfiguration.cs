using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(x => new { x.ProductColorId, x.ImageId });

        builder.HasOne(x => x.ProductColor)
            .WithMany(x => x.ProductImages)
            .HasForeignKey(x => x.ProductColorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Image)
            .WithMany(x => x.ImageProducts)
            .HasForeignKey(x => x.ImageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
