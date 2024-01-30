namespace FreeStuff.Identity.Api;

public static class ApiEndpoints
{
    private const string Prefix = "api/identity";

    public const string Register     = $"{Prefix}/register";
    public const string Login        = $"{Prefix}/login";
    public const string RefreshToken = $"{Prefix}/refresh-token";
    public const string AdminOnly    = $"{Prefix}/admin-only";
}
