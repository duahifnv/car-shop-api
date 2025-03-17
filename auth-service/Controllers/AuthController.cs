using System.ComponentModel.DataAnnotations;
using auth_service;
using auth_service.Dtos;
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
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var token = await _authService.RegisterAsync(request);
            return Ok(new { Token = token });
        }
        catch (Exception ex) when (ex.Message == "Username or email already exists")
        {
            return BadRequest("Username or email already exists");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while registering the user.");
        }
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
    
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromHeader(Name = "Authorization")] string token)
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