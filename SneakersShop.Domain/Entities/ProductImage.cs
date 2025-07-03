using System;

namespace SneakersShop.Domain.Entities;

public class ProductImage
{
    public int ProductColorId { get; set; }
    public int ImageId { get; set; }

    public virtual ProductColor ProductColor { get; set; }
    public virtual File Image { get; set; }
}
