using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using auth_service.Data;
using auth_service.Dtos;
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
    Task<string> RegisterAsync(RegisterRequest request);
    Task<string> AuthenticateAsync(string username, string password);
    Task<UserResponse> GetUserByTokenAsync(string token);
}

public class AuthService : IAuthService
{
    private readonly AuthDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;
    
    public AuthService(AuthDbContext context, IOptions<JwtSettings> jwtSettings, ILogger<AuthService> logger)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        // Проверяем, существует ли пользователь с таким же именем или email
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

        if (existingUser != null)
        {
            throw new Exception("Username or email already exists");
        }
        
        // Создаем нового пользователя
        var user = new User
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Email = request.Email,
            Role = request.Role
        };
        
        _logger.LogInformation("Created new user: '{username}'", user.Username);
        // Добавляем пользователя в базу данных
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User '{username}' saved to database", user.Username);
        // Генерируем JWT-токен
        var token = GenerateJwtToken(user);
        return token;
    }
    
    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new Exception("Invalid username or password");
        }
        _logger.LogInformation("User '{username}' authenticated in system", user.Username);
        var token = GenerateJwtToken(user);
        return token;
    }

    public async Task<UserResponse> GetUserByTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var username = jwtToken.Claims.First(claim => claim.Type == "username").Value;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new Exception("User not found");
        }
        return new UserResponse { Username = user.Username, Email = user.Email };
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
        
        var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
        _logger.LogInformation("Generated new token: {token}..", stringToken.Substring(0, 10));
        return stringToken;
    }
}