namespace FirebaseAuthenticationDotNetCore.Common.Configuration.Security;

public class CookieConfiguration
{
    public const string ConfigSection = "CookieConfig";

    public const string AuthCookieName = "SecurityAuth";

    public const string AuthCookiePath = "/";

    public const string RefreshCookieName = "SecurityRefresh";

    public const string RefreshCookiePath = "/Account/Refresh";

    public string AuthCookieEncryptionKey { get; set; }
}