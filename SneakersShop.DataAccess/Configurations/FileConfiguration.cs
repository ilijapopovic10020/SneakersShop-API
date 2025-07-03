using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class FileConfiguration : EntityConfiguration<Domain.Entities.File>
{
    public override void ConfigureEntity(EntityTypeBuilder<Domain.Entities.File> builder)
    {
        builder.Property(x => x.Path).HasMaxLength(256).IsRequired(true);
        builder.Property(x => x.Size).IsRequired();
    }
}
