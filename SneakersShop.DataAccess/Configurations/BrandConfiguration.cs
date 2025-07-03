using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class BrandConfiguration : EntityConfiguration<Brand>
{
    public override void ConfigureEntity(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(x => x.Name).IsRequired(true);
        builder.HasIndex(x => x.Name);
        
        builder.HasOne(x => x.Image)
               .WithMany()
               .HasForeignKey(x => x.ImageId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
