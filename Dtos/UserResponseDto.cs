namespace MongoCrudApi.Dtos;

// DTO used when sending data back to the client
public record UserResponseDto(
    string Id,
    string Name,
    string Email
);
