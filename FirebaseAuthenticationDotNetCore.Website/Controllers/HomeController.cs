using System.Security.Claims;
using FirebaseAuthenticationDotNetCore.Services.User;
using FirebaseAuthenticationDotNetCore.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseAuthenticationDotNetCore.Website.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly IUserService _userService;

    public HomeController(ClaimsPrincipal claimsPrincipal, IUserService userService)
    {
        _claimsPrincipal = claimsPrincipal;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var currentUserIdentifier = _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userDetails = new UserDetails();
        
        if (!string.IsNullOrWhiteSpace(currentUserIdentifier))
        {
            var userInfo = await _userService.GetBasicUserDetailsAsync(currentUserIdentifier);
        
            if (userInfo.HasValue)
            {
                userDetails.Name = userInfo.Value.Name;
                userDetails.EmailAddress = userInfo.Value.EmailAddress;
                userDetails.PhoneNumber = userInfo.Value.PhoneNumber;
            }
        }

        return View(userDetails);
    }
}