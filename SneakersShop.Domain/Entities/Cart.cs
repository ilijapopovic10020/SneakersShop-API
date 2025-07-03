using System;

namespace SneakersShop.Domain.Entities;

public class Cart : Entity
{
    public int UserId { get; set; }
    public string? CartItems { get; set; }

    public virtual User User { get; set; }
}
