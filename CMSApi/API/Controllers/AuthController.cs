using CMSApi.Core.DTO;
using CMSApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMSApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            try
            {
                var tokenModel = await _authService.Authenticate(loginDTO);

                // Set the access token as an HTTP-only cookie
                Response.Cookies.Append("AccessToken", tokenModel.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30) // Matches token expiration
                });

                // Optionally, you can return the refresh token as well, if you need it on the frontend
                return Ok(new { AccessToken = tokenModel.AccessToken, RefreshToken = tokenModel.RefreshToken });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid username or password");
            }
        }



        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var newToken = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(newToken);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid refresh token");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] TokenModelDTO tokenModel)
        {
            try
            {
                // Call your auth service to revoke the refresh token
                await _authService.RevokeRefreshTokenAsync(tokenModel.RefreshToken);
                return Ok("Logged out successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Logout failed: " + ex.Message);
            }
        }


    }
}
