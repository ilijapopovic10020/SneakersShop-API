using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class ProductConfiguration : EntityConfiguration<Product>
{
    public override void ConfigureEntity(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Brand)
               .WithMany(x => x.Products)
               .HasForeignKey(x => x.BrandId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
               .WithMany(x => x.Products)
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
       
       builder.HasMany(x => x.Reviews)
               .WithOne(x => x.Product)
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
