using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class ColorConfiguration : EntityConfiguration<Color>
{
    public override void ConfigureEntity(EntityTypeBuilder<Color> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
    }
}
