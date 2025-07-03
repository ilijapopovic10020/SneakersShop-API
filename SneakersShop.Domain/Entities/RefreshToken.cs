namespace SneakersShop.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public int UserId { get; set; }
    public string DeviceInfo { get; set; } = default!;

    public User User { get; set; }
}
