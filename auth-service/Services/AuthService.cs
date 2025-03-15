using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using auth_service.Data;
using auth_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace auth_service;

public class JwtSettings
{
    public string Secret { get; set; } // Секретный ключ для подписи токена
    public string Issuer { get; set; } // Издатель токена
    public string Audience { get; set; } // Аудитория токена
    public int ExpiryMinutes { get; set; } // Время жизни токена в минутах
}

public interface IAuthService
{
    Task<string> AuthenticateAsync(string username, string password);
    Task<User> GetUserByTokenAsync(string token);
}

public class AuthService : IAuthService
{
    private readonly AuthDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(AuthDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

        if (user == null)
        {
            throw new Exception("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        return token;
    }

    public async Task<User> GetUserByTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var username = jwtToken.Claims.First(claim => claim.Type == "username").Value;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim("username", user.Username),
            new Claim("role", user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}