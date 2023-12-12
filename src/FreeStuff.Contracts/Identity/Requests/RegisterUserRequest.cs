namespace FreeStuff.Contracts.Identity.Requests;

public record RegisterUserRequest(
    string              FirstName,
    string              LastName,
    string              UserName,
    string              Email,
    string              Password,
    ICollection<string> Roles
);
