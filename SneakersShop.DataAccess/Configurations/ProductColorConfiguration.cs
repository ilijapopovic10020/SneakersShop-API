using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class ProductColorConfiguration : EntityConfiguration<ProductColor>
{

       public override void ConfigureEntity(EntityTypeBuilder<ProductColor> builder)
       {
              builder.Property(x => x.Code).IsRequired();

              builder.HasOne(x => x.Product)
                     .WithMany(x => x.ProductColors)
                     .HasForeignKey(x => x.ProductId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(x => x.Color)
                     .WithMany(x => x.ColorProducts)
                     .HasForeignKey(x => x.ColorId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(x => x.Favorites)
                     .WithOne(x => x.ProductColor)
                     .HasForeignKey(x => x.ProductColorId)
                     .OnDelete(DeleteBehavior.Restrict);
               
               builder.HasMany(x => x.ProductDiscounts)
                     .WithOne(x => x.ProductColor)
                     .HasForeignKey(x => x.ProductColorId)
                     .OnDelete(DeleteBehavior.Restrict);
    }
}
