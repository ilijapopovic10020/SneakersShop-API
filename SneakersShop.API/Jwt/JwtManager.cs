using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SneakersShop.API.Exceptions;
using SneakersShop.DataAccess;
using SneakersShop.Domain.Entities;

namespace SneakersShop.API.Jwt;

public class JwtManager(SneakersShopDbContext context, JwtSettings settings)
{
    private readonly SneakersShopDbContext _context = context;
    private readonly JwtSettings _settings = settings;

    public (string accessToken, string refreshToken) MakeToken(string username, string password, string deviceInfo, bool revokeOldToken = false)
    {
        var user = _context.Users.Include(x => x.Role)
                                 .ThenInclude(r => r.UseCases)
                                 .FirstOrDefault(x => x.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            throw new UserNotFoundException(username);

        if (revokeOldToken)
        {
            var oldTokens = _context.RefreshTokens
                                    .Where(x => x.UserId == user.Id && !x.IsRevoked)
                                    .ToList();
            foreach (var token in oldTokens)
                token.IsRevoked = true;

            _context.SaveChanges();
        }

        var actor = new JwtActor
        {
            Id = user.Id,
            Identity = user.Username,
            Email = user.Email,
            UseCaseIds = [.. user.Role.UseCases.Select(x => x.UseCaseId)]
        };

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _settings.Issuer),
            new(JwtRegisteredClaimNames.Iss, "sneakers_shop_api", ClaimValueTypes.String, _settings.Issuer),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _settings.Issuer),
            new("UseCases", JsonConvert.SerializeObject(actor.UseCaseIds)),
            new("Id", user.Id.ToString(), ClaimValueTypes.String, _settings.Issuer),
            new("Username", user.Username),
            new("Email", user.Email),
            new("Role", user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var jwtToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: "Any",
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_settings.Minutes),
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = now.AddDays(7),
            IsRevoked = false,
            UserId = user.Id,
            DeviceInfo = deviceInfo

        };

        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        return (accessToken, refreshToken.Token);
    }

    public string RefreshAccessToken(string refreshToken)
    {
        var storedToken = _context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.Role)
            .ThenInclude(r => r.UseCases)
            .FirstOrDefault(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                ?? throw new UnauthorizedAccessException("Invalid or expired refresh token");

        var user = storedToken.User;

        var actor = new JwtActor
        {
            Id = user.Id,
            Identity = user.Username,
            Email = user.Email,
            UseCaseIds = [.. user.Role.UseCases.Select(x => x.UseCaseId)]
        };

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _settings.Issuer),
            new(JwtRegisteredClaimNames.Iss, "sneakers_shop_api", ClaimValueTypes.String, _settings.Issuer),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _settings.Issuer),
            new("UseCases", JsonConvert.SerializeObject(actor.UseCaseIds)),
            new("Id", user.Id.ToString(), ClaimValueTypes.String, _settings.Issuer),
            new("Username", user.Username),
            new("Email", user.Email),
            new("Role", user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var jwtToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: "Any",
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_settings.Minutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public void RevokeRefreshToken(string refreshToken)
    {
        var token = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

        if (token == null || token.IsRevoked)
            throw new UnauthorizedAccessException("Token is invalid or already revoked");

        token.IsRevoked = true;
        _context.SaveChanges();
    }

    public void RevokeAllRefreshTokens(string username)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == username);

        if (user == null) return;

        var tokens = _context.RefreshTokens.Where(x => x.UserId == user.Id && !x.IsRevoked).ToList();

        foreach (var token in tokens)
            token.IsRevoked = true;

        _context.SaveChanges();
    }
}
