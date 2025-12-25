using Microsoft.AspNetCore.Mvc;
using MongoCrudApi.Dtos;
using MongoCrudApi.Models;
using MongoCrudApi.Services;

namespace MongoCrudApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public AuthController(UserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash
        };

        await _userService.CreateAsync(user);

        return Ok("User registered successfully");
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userService.GetByEmailAsync(dto.Email);

        if (user is null)
            return Unauthorized("Invalid credentials");

        // Verify password
        bool isValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!isValid)
            return Unauthorized("Invalid credentials");

        // Generate JWT
        var token = _jwtService.GenerateToken(user);

        return Ok(new { token });
    }
}
