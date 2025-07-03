using System;

namespace SneakersShop.Domain.Entities;

public class Size : Entity
{
    public decimal Number { get; set; }
    public string Detail { get; set; }

    public virtual ICollection<ProductSize> SizeProducts { get; set; } = new List<ProductSize>();
}
