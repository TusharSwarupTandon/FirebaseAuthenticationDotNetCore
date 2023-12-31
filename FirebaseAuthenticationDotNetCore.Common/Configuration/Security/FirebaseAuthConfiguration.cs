namespace FirebaseAuthenticationDotNetCore.Common.Configuration.Security;

public class FirebaseAuthConfiguration
{
    public const string ConfigSection = "FirebaseAuthConfig";

    public string ApiKey { get; set; }

    public string AuthDomain { get; set; }

    public string ProjectId { get; set; }

    public string StorageBucket { get; set; }

    public string MessagingSenderId { get; set; }

    public string AppId { get; set; }

    public string MeasurementId { get; set; }

    public string ValidAuthority { get; set; }
}