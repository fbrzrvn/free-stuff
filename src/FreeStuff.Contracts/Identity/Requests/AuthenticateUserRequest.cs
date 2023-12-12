namespace FreeStuff.Contracts.Identity.Requests;

public record AuthenticateUserRequest(string Email, string Password);
