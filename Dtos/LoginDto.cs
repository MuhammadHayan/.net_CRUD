using System.ComponentModel.DataAnnotations;

namespace MongoCrudApi.Dtos;

// Used when user logs in
public record LoginDto(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Password
);
