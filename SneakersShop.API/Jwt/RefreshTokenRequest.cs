using System;

namespace SneakersShop.API.Jwt;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = default!;
}
