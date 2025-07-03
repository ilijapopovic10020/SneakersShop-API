using System;

namespace SneakersShop.Application.UseCases.DTO;

public class CartDto : BaseDto
{
    public SizeDto Size { get; set; }
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
}
