using System;
using SneakersShop.Domain.Entities;

namespace SneakersShop.Application.UseCases.DTO;

public class CreateOrderDto
{
    public PaymentType PaymentType { get; set; }
    public string? Notes { get; set; }
    public string? CardHolder { get; set; }
    public string? CardNumber { get; set; }
    public string? Cvv { get; set; }
    public string? Expiration { get; set; }
    public IEnumerable<CreateOrderItemDto> Items { get; set; } = [];
    public int AddressId { get; set; }
}

public class CreateOrderItemDto
{
    public int ProductColorId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }
}
