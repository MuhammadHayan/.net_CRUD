using System.ComponentModel.DataAnnotations;

namespace MongoCrudApi.Dtos;

// Used when user registers
public record RegisterDto(
    [Required]
    string Name,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(6)]
    string Password
);
