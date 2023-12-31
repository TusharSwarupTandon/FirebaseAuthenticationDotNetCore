using Firebase.Auth;
using FirebaseAuthenticationDotNetCore.Common.Configuration.Security;
using FirebaseAuthenticationDotNetCore.Common.Helper;
using FirebaseAuthenticationDotNetCore.Models;
using Microsoft.Extensions.Options;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;

namespace FirebaseAuthenticationDotNetCore.Services.Security;

public class FirebaseSecurityProvider : ISecurityProvider
{
    private readonly FirebaseAuthProvider _firebaseAuthProvider;
    private readonly FirebaseAuth _firebaseAuth;
    private readonly CookieConfiguration _cookieConfiguration;

    public FirebaseSecurityProvider(IOptions<FirebaseAuthConfiguration> firebaseAuthConfiguration,
        IOptions<CookieConfiguration> cookieConfiguration)
    {
        var firebaseAuthConfiguration1 = firebaseAuthConfiguration.Value;
        _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseAuthConfiguration1.ApiKey));
        _firebaseAuth = FirebaseAuth.DefaultInstance;
        _cookieConfiguration = cookieConfiguration.Value;
    }

    public async Task<UserInfo?> GetBasicUserDetailsAsync(string uid)
    {
        var userDetails = await _firebaseAuth.GetUserAsync(uid);

        if (userDetails is null)
        {
            return null;
        }

        return new UserInfo(userDetails.DisplayName, userDetails.Email, userDetails.PhoneNumber);
    }

    public async Task<AuthenticationResult> GetAuthenticationResultForUserUsingRefreshTokenAsync(string refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        var authResult = await _firebaseAuthProvider.RefreshAuthAsync(new ()
        {
            RefreshToken = refreshToken
        });

        if (string.IsNullOrEmpty(authResult.FirebaseToken))
        {
            return new AuthenticationResult("Invalid Refresh Token");
        }

        return await MapToAuthenticationResult(authResult);
    }

    private async Task<AuthenticationResult> MapToAuthenticationResult(FirebaseAuthLink authResult)
    {
        var encryptedAuthToken =
            await EncryptionHelper.EncryptAsync(authResult.FirebaseToken, _cookieConfiguration.AuthCookieEncryptionKey);

        var authenticationResult = new AuthenticationResult(encryptedAuthToken, authResult.RefreshToken);

        return authenticationResult;
    }
}