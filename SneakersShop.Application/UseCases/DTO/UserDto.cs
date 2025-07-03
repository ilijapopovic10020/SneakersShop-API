using System;

namespace SneakersShop.Application.UseCases.DTO;

public class UserDto : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Image { get; set; }
    public string? Phone { get; set; }
    public IEnumerable<AddressDto> Addresses { get; set; }
}
