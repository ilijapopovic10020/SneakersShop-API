using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.API.Jwt;

namespace SneakersShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(JwtManager manager) : ControllerBase
    {
        private readonly JwtManager _manager = manager;

        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest request, [FromQuery] bool revokeOld = false)
        {
            var (accessToken, refreshToken) = _manager.MakeToken(request.Username, request.Password, request.DeviceInfo, revokeOld);

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        {
            var newAccessToken = _manager.RefreshAccessToken(request.RefreshToken);

            return Ok(new { AccessToken = newAccessToken });
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] RefreshTokenRequest request)
        {
            _manager.RevokeRefreshToken(request.RefreshToken);

            return NoContent();
        }

        [HttpPost("Logout-all")]
        public IActionResult LogoutAll([FromBody] string username)
        {
            _manager.RevokeAllRefreshTokens(username);
            return NoContent();
        }
    }
}
