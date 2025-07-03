using System;

namespace SneakersShop.Domain.Entities;

public class Brand : Entity
{
    public string Name { get; set; }
    public int ImageId { get; set; }

    public virtual File Image { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
