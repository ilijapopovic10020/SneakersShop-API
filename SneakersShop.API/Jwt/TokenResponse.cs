using System;

namespace SneakersShop.API.Jwt;

public class TokenResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
