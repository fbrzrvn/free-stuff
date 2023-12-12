namespace FreeStuff.Identity.Api;

public static class ApiEndpoints
{
    private const string Prefix = "api/identity";

    public const string Register = $"{Prefix}/register";
    public const string Token    = $"{Prefix}/token";
    public const string Me       = $"{Prefix}/me";
}
