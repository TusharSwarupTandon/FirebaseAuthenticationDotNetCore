using FirebaseAuthenticationDotNetCore.Models;
using FirebaseAuthenticationDotNetCore.Services.Security;

namespace FirebaseAuthenticationDotNetCore.Services.User;

public class UserService : IUserService
{
    private readonly ISecurityService _securityService;

    public UserService(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<UserInfo?> GetBasicUserDetailsAsync(string uid)
    {
        return await _securityService.GetBasicUserDetailsAsync(uid);
    }
}