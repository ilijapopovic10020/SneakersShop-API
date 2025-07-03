using System;

namespace SneakersShop.Domain.Entities;

public class Address : Entity
{
    public int UserId { get; set; }
    public string Street { get; set; }
    public int CityId { get; set; }
    public bool IsDefault { get; set; }

    public virtual User User { get; set; }
    public virtual City City { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = [];
}
