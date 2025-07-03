using System;

namespace SneakersShop.Domain;

public interface IApplicationUser
{
    public string Identity { get; }
    public int Id { get; }
    public IEnumerable<int> UseCaseIds { get; }
    public string Email { get; }
    public string Role { get; }
}
