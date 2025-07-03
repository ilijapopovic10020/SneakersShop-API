using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class FavoriteConfiguration : EntityConfiguration<Favorite>
{
    public override void ConfigureEntity(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasOne(x => x.ProductColor)
               .WithMany()
               .HasForeignKey(x => x.ProductColorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}