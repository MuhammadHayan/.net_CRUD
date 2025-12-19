using System.ComponentModel.DataAnnotations;

namespace MongoCrudApi.Dtos;

// DTO used when creating a user (incoming request)
public record CreateUserDto(
    [Required]
    [MinLength(2)]
    string Name,

    [Required]
    [EmailAddress]
    string Email
);
