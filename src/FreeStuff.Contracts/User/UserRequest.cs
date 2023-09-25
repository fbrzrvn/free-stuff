namespace FreeStuff.Contracts.User;

public record UserRequest
(
    string FullName,
    string Username,
    string Email,
    string Password
);