using System;

namespace SneakersShop.Application.UseCases.DTO;

public class CityDto : BaseDto
{
    public string Name { get; set; }
    public string ZipCode { get; set; }
}
