using System;

namespace SneakersShop.Domain.Entities;

public class Review : Entity
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }


    public virtual Product Product { get; set; }
    public virtual User User { get; set; }
}
