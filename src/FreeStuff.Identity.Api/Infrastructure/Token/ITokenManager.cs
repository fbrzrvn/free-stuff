using FreeStuff.Contracts.Identity.Responses;
using FreeStuff.Identity.Api.Domain;

namespace FreeStuff.Identity.Api.Infrastructure.Token;

public interface ITokenManager
{
    TokenResponse GenerateTokens(User user, IEnumerable<string> roles);
}
