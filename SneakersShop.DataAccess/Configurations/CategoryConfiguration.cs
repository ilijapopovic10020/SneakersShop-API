using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;
public class CategoryConfiguration : EntityConfiguration<Category>
{
    public override void ConfigureEntity(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(128).IsRequired();
    }
}
