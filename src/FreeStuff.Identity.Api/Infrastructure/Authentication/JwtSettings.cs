namespace FreeStuff.Identity.Api.Infrastructure.Authentication;

public class JwtSettings
{
    public string Secret        { get; init; } = string.Empty;
    public int    ExpiryMinutes { get; init; }
    public string Issuer        { get; init; } = string.Empty;
    public string Audience      { get; init; } = string.Empty;
}
