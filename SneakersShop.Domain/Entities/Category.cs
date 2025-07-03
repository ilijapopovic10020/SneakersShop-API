using System;

namespace SneakersShop.Domain.Entities;

public class Category : Entity
{
    public string Name { get; set; } //male, female, child

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
