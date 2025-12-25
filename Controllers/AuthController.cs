using Microsoft.AspNetCore.Mvc;
using MongoCrudApi.Dtos;
using MongoCrudApi.Models;
using MongoCrudApi.Services;
using MongoCrudApi.Responses;

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

    // ========================
    // REGISTER
    // ========================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // 🔎 Check if email already exists
        var existingUser = await _userService.GetByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Email already registered"
            });
        }

        // 🔐 Hash password before saving
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // DTO → Mongo model
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash
        };

        await _userService.CreateAsync(user);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User registered successfully"
        });
    }

    // ========================
    // LOGIN
    // ========================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        // 🔎 Find user by email
        var user = await _userService.GetByEmailAsync(dto.Email);

        if (user is null)
        {
            return Unauthorized(new ApiResponse
            {
                Success = false,
                Message = "Invalid email or password"
            });
        }

        // 🔐 Verify password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!isPasswordValid)
        {
            return Unauthorized(new ApiResponse
            {
                Success = false,
                Message = "Invalid email or password"
            });
        }

        // 🔑 Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = new
            {
                token
            }
        });
    }
}
