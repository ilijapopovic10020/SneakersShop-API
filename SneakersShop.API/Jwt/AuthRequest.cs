using System;

namespace SneakersShop.API.Jwt;

public class AuthRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string DeviceInfo { get; set; }
}
