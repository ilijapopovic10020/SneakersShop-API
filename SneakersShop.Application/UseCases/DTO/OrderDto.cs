using System;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Application.UseCases.DTO;

public class OrderDto : BaseDto
{
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? PromisedDate { get; set; }
    public string? EstimatedArrival { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public IEnumerable<OrderItemDto> Items { get; set; } = [];
}

public class OrderItemDto : BaseDto
{
    public int ProductColorId { get; set; }
    public string Image { get; set; } = string.Empty;
}
