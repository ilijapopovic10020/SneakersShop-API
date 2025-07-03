using System;

namespace SneakersShop.Domain.Entities;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActive { get; set; }
    public string? DeletedBy { get; set; }
    public string? ModifiedBy { get; set; }
}
