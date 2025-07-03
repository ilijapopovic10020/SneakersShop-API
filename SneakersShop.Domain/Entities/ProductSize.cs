using System;

namespace SneakersShop.Domain.Entities;

public class ProductSize : Entity
{
    public int ProductColorId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }

    public virtual ProductColor ProductColor { get; set; }
    public virtual Size Size { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}
