using System;

namespace SneakersShop.Domain.Entities;

public class User : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public int? ImageId { get; set; }
    public string? Phone { get; set; }

    public virtual Role Role { get; set; }
    public virtual File Image { get; set; }
    public virtual ICollection<Address> Addresses { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
    public virtual ICollection<Favorite> Favorites { get; set; } = [];
    public virtual ICollection<Order> Orders { get; set; } = [];
    public virtual Cart Cart { get; set; }
}
