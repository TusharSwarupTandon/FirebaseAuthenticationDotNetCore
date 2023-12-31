using FirebaseAuthenticationDotNetCore.Common.Configuration.Security;
using FirebaseAuthenticationDotNetCore.Services.Security;
using FirebaseAuthenticationDotNetCore.Website.Models;
using FirebaseAuthenticationDotNetCore.Website.Models.Ajax;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseAuthenticationDotNetCore.Website.Controllers;

public class AccountController(ISecurityService securityService) : Controller
{
    [HttpGet]
    public ActionResult Login(string returnUrl, bool wasRefreshAttempted)
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {
            return HandleUserAuthenticated(returnUrl);
        }

        if (!wasRefreshAttempted)
        {
            return RedirectToAction("Refresh", new { returnUrl });
        }

        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel)
    {
        var authenticationResult = await securityService.AuthenticateUserUsingRefreshTokenAsync(loginViewModel.SecurityToken);

        if (authenticationResult.IsAuthenticationSuccessful)
        {
            UpdateAccessToken(authenticationResult.AuthToken);
            UpdateRefreshToken(authenticationResult.RefreshToken);

            return Json(new LoginResult());
        }

        var response = new LoginResult
        {
            ErrorMessage = authenticationResult.ErrorMessage,
        };

        return Json(response);
    }

    [HttpGet, HttpPost]
    public async Task<ActionResult> RefreshAsync(string returnUrl)
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {
            return HandleUserAuthenticated(returnUrl);
        }

        var refreshCookie = Request.Cookies[CookieConfiguration.RefreshCookieName];
        
        if (!string.IsNullOrWhiteSpace(refreshCookie))
        {
            var authenticationResult = await securityService.AuthenticateUserUsingRefreshTokenAsync(refreshCookie);

            if (authenticationResult.IsAuthenticationSuccessful)
            {
                UpdateAccessToken(authenticationResult.AuthToken);
            }
        }

        return RedirectToAction("Login", new { returnUrl, wasRefreshAttempted = true });
    }

    [HttpPost]
    public async Task<ActionResult> LogoutAsync()
    {
        Response.Cookies.Delete(CookieConfiguration.AuthCookieName);
        Response.Cookies.Delete(CookieConfiguration.RefreshCookieName, new CookieOptions
        {
            Path = "/Account/Refresh"
        });

        return RedirectToAction("Login");
    }

    private ActionResult HandleUserAuthenticated(string returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    private void UpdateAccessToken(string accessToken)
    {
        Response.Cookies.Delete(CookieConfiguration.AuthCookieName);
        
        Response.Cookies.Append(CookieConfiguration.AuthCookieName, accessToken, new CookieOptions
        {
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
        });
    }

    private void UpdateRefreshToken(string refreshToken)
    {
        Response.Cookies.Delete(CookieConfiguration.RefreshCookieName);
        Response.Cookies.Append(CookieConfiguration.RefreshCookieName, refreshToken, new CookieOptions
        {
            Path = "/Account/Refresh",
            Expires = DateTimeOffset.UtcNow.AddDays(90),
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
        });
    }
}