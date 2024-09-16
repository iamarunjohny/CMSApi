using CMSApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly MyAppDbContext _context;

        public UserLoginController(MyAppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST api/users/login - Login and issue tokens
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Invalid login details.");
            }

            // Find the user by username
            var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);
            if (user == null || user.Passwords != login.Password)
            {
                return BadRequest("Invalid username or password.");
            }

            // Generate access token
            var accessToken = GenerateAccessToken(login.Username);

            // Generate and store refresh token
            var refreshToken = GenerateRefreshToken();
            SaveRefreshToken(user.Username, refreshToken);

            // Set the refresh token as an HttpOnly cookie
            SetRefreshTokenCookie(refreshToken);

            return Ok(new
            {
                AccessToken = accessToken
            });
        }

        // POST api/users/refresh-token - Refresh tokens
        [HttpPost("refresh-token")]
        public IActionResult Refresh([FromBody] TokenModel tokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            var username = principal.Identity.Name;

            // Check if the provided refresh token is valid and still stored
            var storedToken = _context.RefreshTokens.SingleOrDefault(t => t.Token == tokenModel.RefreshToken);
            if (storedToken == null || storedToken.ExpiryDate <= DateTime.Now)
            {
                return BadRequest("Invalid or expired refresh token.");
            }

            // Generate new access and refresh tokens
            var newAccessToken = GenerateAccessToken(username);
            var newRefreshToken = GenerateRefreshToken();

            // Rotate the refresh token (replace with new one)
            storedToken.Token = newRefreshToken;
            storedToken.ExpiryDate = DateTime.Now.AddDays(7);
            _context.SaveChanges();

            // Set the new refresh token in HttpOnly cookie
            SetRefreshTokenCookie(newRefreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken
            });
        }

        // POST api/users/logout - Logout and revoke refresh token
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] TokenModel tokenModel)
        {
            // Find and remove the refresh token from the database
            var refreshToken = _context.RefreshTokens.SingleOrDefault(t => t.Token == tokenModel.RefreshToken);
            if (refreshToken == null)
            {
                return BadRequest("Invalid refresh token.");
            }

            _context.RefreshTokens.Remove(refreshToken);
            _context.SaveChanges();

            return Ok("Logged out successfully.");
        }

        // Helper Methods
        private string GenerateAccessToken(string username)
        {
            // Security key and signing credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims (you can add more claims if needed)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create JWT Token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Access token expires in 30 minutes
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            // You can use any method to generate a secure refresh token
            return Guid.NewGuid().ToString();
        }

        private void SaveRefreshToken(string username, string refreshToken)
        {
            var token = new RefreshToken
            {
                Username = username,
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(7) // Set expiry for 7 days
            };

            _context.RefreshTokens.Add(token);
            _context.SaveChanges();
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // HttpOnly ensures it's not accessible via JavaScript
                Expires = DateTime.Now.AddDays(7) // Match cookie expiration with token expiration
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false // Don't check for token expiry when extracting claims
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
