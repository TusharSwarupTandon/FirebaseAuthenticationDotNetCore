using FirebaseAuthenticationDotNetCore.Models;

namespace FirebaseAuthenticationDotNetCore.Services.Security;

public interface ISecurityProvider
{
    Task<UserInfo?> GetBasicUserDetailsAsync(string uid);

    Task<AuthenticationResult> GetAuthenticationResultForUserUsingRefreshTokenAsync(string refreshToken);
}