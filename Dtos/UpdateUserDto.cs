using System.ComponentModel.DataAnnotations;

namespace MongoCrudApi.Dtos;

// DTO used when updating a user
public record UpdateUserDto(
    [Required]
    [MinLength(2)]
    string Name,

    [Required]
    [EmailAddress]
    string Email
);
