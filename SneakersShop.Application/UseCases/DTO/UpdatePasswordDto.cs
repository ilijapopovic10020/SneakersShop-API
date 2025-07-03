using System;

namespace SneakersShop.Application.UseCases.DTO;

public class UpdatePasswordDto : UpdateEntityDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
