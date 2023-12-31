namespace FirebaseAuthenticationDotNetCore.Common.Configuration.Security;

public class CookieConfiguration
{
    public const string ConfigSection = "CookieConfig";

    public const string AuthCookieName = "SecurityAuth";

    public const string RefreshCookieName = "SecurityRefresh";

    public string AuthCookieEncryptionKey { get; set; }
}