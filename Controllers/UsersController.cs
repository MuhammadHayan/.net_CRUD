using Microsoft.AspNetCore.Mvc;
using MongoCrudApi.Dtos;
using MongoCrudApi.Models;
using MongoCrudApi.Services;

namespace MongoCrudApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> Get()
    {
        var users = await _userService.GetAsync();

        // Map Mongo model -> Response DTO
        var response = users.Select(u => new UserResponseDto(
            u.Id!,
            u.Name,
            u.Email
        )).ToList();

        return Ok(response);
    }

    // GET: api/users/{id}
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserResponseDto>> Get(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        var response = new UserResponseDto(
            user.Id!,
            user.Name,
            user.Email
        );

        return Ok(response);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto dto)
    {
        // DTO -> Mongo model
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _userService.CreateAsync(user);

        // Mongo model -> Response DTO
        var response = new UserResponseDto(
            user.Id!,
            user.Name,
            user.Email
        );

        return CreatedAtAction(nameof(Get), new { id = user.Id }, response);
    }

    // PUT: api/users/{id}
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UpdateUserDto dto)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        // Update allowed fields only
        user.Name = dto.Name;
        user.Email = dto.Email;

        await _userService.UpdateAsync(id, user);

        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        await _userService.RemoveAsync(id);

        return NoContent();
    }
}
