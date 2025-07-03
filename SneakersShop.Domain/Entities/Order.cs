using System;

namespace SneakersShop.Domain.Entities;

public class Order : Entity
{
    public int UserId { get; set; }
    public int? AddressId { get; set; }
    public decimal TotalPrice { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime PromisedDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string? Notes { get; set; }


    public virtual User User { get; set; }
    public virtual Address Address { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}
