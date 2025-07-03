using System;

namespace SneakersShop.Domain.Entities;

public class Color : Entity
{
    public string Name { get; set; }

    public virtual ICollection<ProductColor> ColorProducts { get; set; } = new List<ProductColor>();
}
