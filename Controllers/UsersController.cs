using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoCrudApi.Dtos;
using MongoCrudApi.Models;
using MongoCrudApi.Services;
using MongoCrudApi.Responses;

namespace MongoCrudApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // 🔒 Requires valid JWT token
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userService.GetAsync();

        // Mongo model → DTO
        var responseData = users.Select(u => new UserResponseDto(
            u.Id!,
            u.Name,
            u.Email
        )).ToList();

        return Ok(new ApiResponse<List<UserResponseDto>>
        {
            Success = true,
            Data = responseData
        });
    }

    // GET: api/users/{id}
    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "User not found"
            });
        }

        var responseData = new UserResponseDto(
            user.Id!,
            user.Name,
            user.Email
        );

        return Ok(new ApiResponse<UserResponseDto>
        {
            Success = true,
            Data = responseData
        });
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto dto)
    {
        // DTO → Mongo model
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _userService.CreateAsync(user);

        var responseData = new UserResponseDto(
            user.Id!,
            user.Name,
            user.Email
        );

        return CreatedAtAction(nameof(Get), new { id = user.Id },
            new ApiResponse<UserResponseDto>
            {
                Success = true,
                Data = responseData
            });
    }

    // PUT: api/users/{id}
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UpdateUserDto dto)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "User not found"
            });
        }

        // Only allowed fields updated
        user.Name = dto.Name;
        user.Email = dto.Email;

        await _userService.UpdateAsync(id, user);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User updated successfully"
        });
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "User not found"
            });
        }

        await _userService.RemoveAsync(id);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User deleted successfully"
        });
    }
}
