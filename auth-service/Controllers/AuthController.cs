using System.ComponentModel.DataAnnotations;
using auth_service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // [Authorize("AdminOnly")]
    [HttpGet("user")]
    public async Task<IActionResult> GetUser([FromHeader(Name = "Authorization")] string token)
    {
        try
        {
            var user = await _authService.GetUserByTokenAsync(token.Replace("Bearer ", ""));
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class LoginRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}