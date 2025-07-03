using System;

namespace SneakersShop.Domain.Entities;

public class File : Entity
{
    public string Path { get; set; }
    public int Size { get; set; }

    public virtual ICollection<ProductImage> ImageProducts { get; set; } = new List<ProductImage>();
}
