using System;

namespace SneakersShop.API.Jwt;

public class JwtSettings
{
    public int Minutes { get; set; }
    public string Issuer { get; set; }
    public string SecretKey { get; set; }
}
