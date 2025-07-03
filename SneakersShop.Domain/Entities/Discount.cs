using System;
using System.Diagnostics.Contracts;

namespace SneakersShop.Domain.Entities;

public class Discount : Entity
{
    public string Name { get; set; }
    public decimal Percentage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}
