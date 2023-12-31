using FirebaseAuthenticationDotNetCore.Models;

namespace FirebaseAuthenticationDotNetCore.Services.Security;

public interface ISecurityService
{
    Task<UserInfo?> GetBasicUserDetailsAsync(string uid);

    Task<AuthenticationResult> AuthenticateUserUsingRefreshTokenAsync(string refreshToken);
}