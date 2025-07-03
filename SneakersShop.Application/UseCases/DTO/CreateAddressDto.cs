using System;

namespace SneakersShop.Application.UseCases.DTO;

public class CreateAddressDto
{
    public int CityId { get; set; }
    public string Street { get; set; }
}
