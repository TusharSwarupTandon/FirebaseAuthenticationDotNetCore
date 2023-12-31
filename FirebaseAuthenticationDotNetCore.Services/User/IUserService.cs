using FirebaseAuthenticationDotNetCore.Models;

namespace FirebaseAuthenticationDotNetCore.Services.User;

public interface IUserService
{
     Task<UserInfo?> GetBasicUserDetailsAsync(string uid);
}