using CMSApi.Core.DTO;
using CMSApi.Core.Entities;
using CMSApi.Core.Interfaces;
using CMSApi.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMSApi.BLL
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly MyAppDbContext _context;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;

        public AuthService(IUserRepository userRepository, MyAppDbContext context, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _context = context;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
        }

        // Authenticate and generate tokens
        public async Task<TokenModelDTO> Authenticate(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(userLoginDTO.Username);

            if (user == null || user.Passwords != userLoginDTO.Password)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Generate access and refresh tokens
            return GenerateTokens(user);
        }




        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            // Find and remove the refresh token from the database
            var token = await _context.RefreshTokens.SingleOrDefaultAsync(t => t.Token == refreshToken);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }



        // Refresh the access token using the refresh token
        public async Task<TokenModelDTO> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenRecord = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiryDate > DateTime.Now);

            if (refreshTokenRecord == null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = await _userRepository.GetByUsernameAsync(refreshTokenRecord.Username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            return GenerateTokens(user);  // Re-generate access token using valid refresh token
        }

        // Generate access token and refresh token
        private TokenModelDTO GenerateTokens(User user)
        {
            // Define claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            // Generate access token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),  // Access token expires in 30 minutes
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Generate refresh token
            var refreshToken = Guid.NewGuid().ToString();

            // Store refresh token in the database
            var refreshTokenRecord = new RefreshToken
            {
                Token = refreshToken,
                Username = user.Username,
                ExpiryDate = DateTime.Now.AddDays(7) // Refresh token valid for 7 days
            };
            _context.RefreshTokens.Add(refreshTokenRecord);
            _context.SaveChanges();

            return new TokenModelDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
