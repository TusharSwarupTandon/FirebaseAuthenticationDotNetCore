namespace FirebaseAuthenticationDotNetCore.Models;

public class AuthenticationResult
{
    public AuthenticationResult(string authToken, string refreshToken)
    {
        AuthToken = authToken;
        RefreshToken = refreshToken;
    }

    public AuthenticationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public string AuthToken { get; }

    public string RefreshToken { get; }
    
    public string ErrorMessage { get; }

    public bool IsAuthenticationSuccessful =>
        string.IsNullOrWhiteSpace(ErrorMessage) && !string.IsNullOrWhiteSpace(AuthToken);
}