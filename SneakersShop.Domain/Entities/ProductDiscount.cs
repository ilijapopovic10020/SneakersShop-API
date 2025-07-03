using System;

namespace SneakersShop.Domain.Entities;

public class ProductDiscount
{
    public int ProductColorId { get; set; }

    public int DiscountId { get; set; }

    public virtual ProductColor ProductColor { get; set; }
    public virtual Discount Discount { get; set; }
}
