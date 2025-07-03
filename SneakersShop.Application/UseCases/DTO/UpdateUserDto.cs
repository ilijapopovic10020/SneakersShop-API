using System;

namespace SneakersShop.Application.UseCases.DTO;

public class UpdateUserDto : UpdateEntityDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public string Phone { get; set; }
}
