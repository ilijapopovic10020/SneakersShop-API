using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SneakersShop.Domain;
using SneakersShop.Domain.Entities;

namespace SneakersShop.DataAccess;

public class SneakersShopDbContext : DbContext
{
    public IApplicationUser User { get; }

    public SneakersShopDbContext(IApplicationUser user = null)
    {
        User = user;
    }

    public SneakersShopDbContext()
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        modelBuilder.Entity<UserUseCase>().HasKey(x => new { x.RoleId, x.UseCaseId });
        modelBuilder.Entity<ProductDiscount>().HasKey(pd => new { pd.ProductColorId, pd.DiscountId });

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=ILIJA\SQLEXPRESS;Initial Catalog=SneakersShopdb;Integrated Security=True;Trust Server Certificate=True");
    }

    public override int SaveChanges()
    {
        foreach (var entry in this.ChangeTracker.Entries())
        {
            if (entry.Entity is Entity e)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        e.IsActive = true;
                        e.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        e.ModifiedAt = DateTime.UtcNow;
                        e.ModifiedBy = User.Identity;
                        break;
                }
            }
        }

        return base.SaveChanges();
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Domain.Entities.File> Files { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductSize> ProductSizes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserUseCase> UserUseCases { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<ProductDiscount> ProductDiscounts { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
