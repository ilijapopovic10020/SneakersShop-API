using System;

namespace SneakersShop.Application.UseCases.DTO;

public class CreateUserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Image { get; set; }
    public required string Phone { get; set; }
}
