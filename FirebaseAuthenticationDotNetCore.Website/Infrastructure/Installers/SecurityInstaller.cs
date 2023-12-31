using System.Text;
using FirebaseAdmin;
using FirebaseAuthenticationDotNetCore.Common.Configuration.Security;
using FirebaseAuthenticationDotNetCore.Common.Constants;
using FirebaseAuthenticationDotNetCore.Common.Helper;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FirebaseAuthenticationDotNetCore.Website.Infrastructure.Installers;

public static class SecurityInstaller
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var cookieConfiguration = new CookieConfiguration();
        var firebaseAuthConfiguration = new FirebaseAuthConfiguration();
        var firebaseServiceAccountConfiguration = new FirebaseServiceAccountConfiguration();
        
        configuration.GetSection(CookieConfiguration.ConfigSection).Bind(cookieConfiguration);
        configuration.GetSection(FirebaseAuthConfiguration.ConfigSection)
            .Bind(firebaseAuthConfiguration);
        configuration.GetSection(FirebaseServiceAccountConfiguration.ConfigSection)
            .Bind(firebaseServiceAccountConfiguration);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = firebaseAuthConfiguration.ValidAuthority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = firebaseAuthConfiguration.ValidAuthority,
                ValidAudience = firebaseAuthConfiguration.ProjectId,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(firebaseServiceAccountConfiguration.PrivateKey)),
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies[CookieConfiguration.AuthCookieName];
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        context.Token = EncryptionHelper.DecryptAsync(token, cookieConfiguration.AuthCookieEncryptionKey)
                            .Result;
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var redirectUrl = UrlConstants.RelativeLoginUrl;
                    var requestPath = context.Request.Path;
                    if (!string.IsNullOrWhiteSpace(requestPath))
                    {
                        redirectUrl += "?returnUrl=" + requestPath;
                    }
                    context.HandleResponse();
                    context.Response.Redirect(redirectUrl);
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void AddSecurityProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var firebaseServiceAccountConfiguration = new FirebaseServiceAccountConfiguration();
        
        configuration.GetSection(FirebaseServiceAccountConfiguration.ConfigSection)
            .Bind(firebaseServiceAccountConfiguration);
        
        services.AddSingleton(FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseServiceAccountConfiguration)),
        }));
    }
}