using System;

namespace SneakersShop.Application.UseCases.DTO;

public class SizeDto : BaseDto
{
    public decimal Number { get; set; }
    public string Detail { get; set; }
    public int Quantity { get; set; }
}
