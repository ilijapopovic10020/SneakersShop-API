using System;

namespace SneakersShop.Domain.Entities;

public class ProductColor : Entity
{
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public string Code { get; set; }

    public virtual Product Product { get; set; }
    public virtual Color Color { get; set; }
    public virtual ICollection<ProductSize> ProductSizes { get; set; } = [];
    public virtual ICollection<ProductImage> ProductImages { get; set; } = [];
    public virtual ICollection<Favorite> Favorites { get; set; } = [];
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}
