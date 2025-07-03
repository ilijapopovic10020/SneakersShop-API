using System;

namespace SneakersShop.Domain.Entities;

public class Role : Entity
{
    public string Name { get; set; }

    public virtual ICollection<UserUseCase> UseCases { get; set; }
}
