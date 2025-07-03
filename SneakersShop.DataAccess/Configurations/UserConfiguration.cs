using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(30);

        builder.HasIndex(x => x.FirstName);
        builder.HasIndex(x => x.LastName);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Username).IsUnique();

        builder.HasOne(x => x.Image)
               .WithMany()
               .HasForeignKey(x => x.ImageId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Reviews)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
               
        builder.HasMany(x => x.Favorites)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)               
               .OnDelete(DeleteBehavior.Restrict);
       builder.HasMany(x => x.Orders)
              .WithOne(x => x.User)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
