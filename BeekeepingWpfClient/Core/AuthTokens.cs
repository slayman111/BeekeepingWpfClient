namespace BeekeepingWpfClient.Core;

public static class AuthTokens
{
    public static string? Access { get; private set; }
    public static string? Refresh { get; private set; }

    static AuthTokens()
    {
        Access = null;
        Refresh = null;
    }

    public static void SetTokens(string? access, string? refresh) =>
        (Access, Refresh) = (access, refresh);
}
