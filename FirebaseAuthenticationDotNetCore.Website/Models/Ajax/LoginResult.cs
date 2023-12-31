namespace FirebaseAuthenticationDotNetCore.Website.Models.Ajax;

public class LoginResult
{
    public string ErrorMessage { get; set; }

    public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
}