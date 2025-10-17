using System;

namespace SneakersShop.Application.UseCases.DTO;

public class AddressesDto : BaseDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public bool IsDefault { get; set; }
}
