using System;

namespace SneakersShop.Domain.Entities;

public class OrderItem : Entity
{
    public int OrderId { get; set; }
    public int ProductSizeId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public virtual Order Order { get; set; }
    public virtual ProductSize ProductSize { get; set; }
}
