using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess.Configurations;

public class CartConfiguration : EntityConfiguration<Cart>
{
    public override void ConfigureEntity(EntityTypeBuilder<Cart> builder)
    {
        builder.Property(x => x.CartItems).HasColumnType("nvarchar(max)");

        builder.HasIndex(c => c.UserId).IsUnique();

        builder.HasOne(x => x.User)
               .WithOne(x => x.Cart)
               .HasForeignKey<Cart>(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
