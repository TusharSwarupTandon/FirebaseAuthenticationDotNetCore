using System.Security.Claims;
using FirebaseAuthenticationDotNetCore.Common.Configuration.Security;
using FirebaseAuthenticationDotNetCore.Services.Security;
using FirebaseAuthenticationDotNetCore.Services.User;
using FirebaseAuthenticationDotNetCore.Website.Infrastructure.Installers;

namespace FirebaseAuthenticationDotNetCore.Website;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddSession();
        MapConfigToObjects(services, configuration);
        services.AddHttpContextAccessor();
        AddServicesToIoc(services);
        services.AddSecurityProvider(configuration);
        services.AddAuthentication(configuration);
        services.AddAuthorization();
    }

    private static void AddServicesToIoc(IServiceCollection services)
    {
        services.AddTransient<ClaimsPrincipal>(s =>
            s.GetService<IHttpContextAccessor>().HttpContext.User);

        services.AddScoped<ISecurityProvider, FirebaseSecurityProvider>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IUserService, UserService>();
    }

    private static void MapConfigToObjects(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CookieConfiguration>(configuration.GetSection(CookieConfiguration.ConfigSection));

        services.Configure<FirebaseAuthConfiguration>(
            configuration.GetSection(FirebaseAuthConfiguration.ConfigSection));

        services.Configure<FirebaseServiceAccountConfiguration>(
            configuration.GetSection(FirebaseServiceAccountConfiguration.ConfigSection));
    }
}