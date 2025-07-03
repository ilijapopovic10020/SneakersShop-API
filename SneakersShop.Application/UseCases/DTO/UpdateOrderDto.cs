using System;

namespace SneakersShop.Application.UseCases.DTO;

public class UpdateOrderDto : UpdateEntityDto
{
    public IEnumerable<CreateOrderItemDto> Items { get; set; }
    public int AddressId { get; set; }
}
public class UpdateOrderItemDto
{
    public int ProductColorId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }
}