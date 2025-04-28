using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CA2.Models;
using CA2.Data;
using Microsoft.Extensions.Configuration;

namespace CA2.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        User Authenticate(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly FootballContext _context;

        public AuthService(IConfiguration configuration, FootballContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            user.Token = GenerateToken(user);
            return user;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // In a real application, use a proper password hashing library like BCrypt
            return password == storedHash; // This is just for demonstration
        }
    }
} 