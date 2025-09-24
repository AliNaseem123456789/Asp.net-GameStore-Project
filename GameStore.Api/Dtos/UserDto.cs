namespace GameStore.Api.Dtos;
public record class UserDto(
    string Username,
    string Email,
    string Password
);
