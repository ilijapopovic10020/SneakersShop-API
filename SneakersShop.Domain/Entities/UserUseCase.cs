using System;

namespace SneakersShop.Domain.Entities;

public class UserUseCase
{
    public int RoleId { get; set; }
    public int UseCaseId { get; set; }

    public virtual Role Role { get; set; }
}
