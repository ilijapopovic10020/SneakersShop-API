using System;

namespace SneakersShop.Application.UseCases.DTO;

public class UpdateAddressDto : UpdateEntityDto
{
    public string Street { get; set; }
    public int CityId { get; set; }
    public bool IsDefault { get; set; }
}
