using FirebaseAuthenticationDotNetCore.Models;

namespace FirebaseAuthenticationDotNetCore.Services.Security;

public class SecurityService : ISecurityService
{
    private readonly ISecurityProvider _securityProvider;

    public SecurityService(ISecurityProvider securityProvider)
    {
        _securityProvider = securityProvider;
    }

    public async Task<UserInfo?> GetBasicUserDetailsAsync(string uid)
    {
        return await _securityProvider.GetBasicUserDetailsAsync(uid);
    }

    public async Task<AuthenticationResult> AuthenticateUserUsingRefreshTokenAsync(string refreshToken)
    {
        return await _securityProvider.GetAuthenticationResultForUserUsingRefreshTokenAsync(refreshToken);
    }
}