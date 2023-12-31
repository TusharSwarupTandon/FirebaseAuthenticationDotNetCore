using Microsoft.AspNetCore.Antiforgery;

namespace FirebaseAuthenticationDotNetCore.Website.Infrastructure.Extensions;

public static class HttpContextExtensions
{
    public static string GetAntiforgeryToken(this HttpContext httpContext)
    {
        var antiforgery = (IAntiforgery)httpContext.RequestServices.GetService(typeof(IAntiforgery));
        var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
        string fieldName = tokenSet.FormFieldName;
        string requestToken = tokenSet.RequestToken;
        return requestToken;
    }
}