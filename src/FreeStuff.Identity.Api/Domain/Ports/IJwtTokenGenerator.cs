namespace FreeStuff.Identity.Api.Domain.Ports;

public interface IJwtTokenGenerator
{
    string GenerateJwt(User user, IEnumerable<string> roles);
}
