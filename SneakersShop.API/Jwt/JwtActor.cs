using System;
using SneakersShop.Domain;

namespace SneakersShop.API.Jwt;

public class JwtActor : IApplicationUser
{
    public string Identity { get; set; }
    public int Id { get; set; }
    public IEnumerable<int> UseCaseIds { get; set; } = [];
    public string Email { get; set; }
    public string Role { get; set; }
}

public class AnonymousActor : IApplicationUser
{
    public string Identity => "Anonymous";
    public int Id => 0;
    public IEnumerable<int> UseCaseIds => [4, 1, 3, 5, 6, 8, 9, 10, 11, 12, 13, 15, 22];
    public string Email => "anonymous@gmail.com";
    public string Role => "user";
}