namespace SneakersShop.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }

    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<ProductColor> ProductColors { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
}
