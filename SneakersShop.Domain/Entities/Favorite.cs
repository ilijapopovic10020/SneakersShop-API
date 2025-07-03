using System;

namespace SneakersShop.Domain.Entities;

public class Favorite : Entity
{
    public int UserId { get; set; }
    public int ProductColorId { get; set; }

    public virtual User User { get; set; }
    public virtual ProductColor ProductColor { get; set; }
}
