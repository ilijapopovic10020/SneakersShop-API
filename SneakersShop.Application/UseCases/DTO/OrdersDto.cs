using System;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Application.UseCases.DTO;

public class OrdersDto : BaseDto
{
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public IEnumerable<OrdersItemsDto> Items { get; set; } = [];
}

public class OrdersItemsDto
{
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Size { get; set; }
    public int Quantity { get; set; }
}