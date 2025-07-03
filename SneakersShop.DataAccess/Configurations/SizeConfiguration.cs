using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class SizeConfiguration : EntityConfiguration<Size>
{
    public override void ConfigureEntity(EntityTypeBuilder<Size> builder)
    {
        builder.Property(x => x.Number).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Detail).IsRequired();
    }
}
