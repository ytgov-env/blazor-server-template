using BlazorServerTemplate.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using MudBlazor.Services;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var options = new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
    };
    var builder = WebApplication.CreateBuilder(options);
    builder.Host.UseWindowsService();

    var configuration = builder.Configuration;

    builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddSingleton(configuration);
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();

    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    // Add authentication services
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = $"/{configuration["AuthNProvider:LoginPath"]}";
        options.LogoutPath = $"/{configuration["AuthNProvider:LogoutPath"]}";
    })
    .AddOpenIdConnect(configuration["AuthNProvider:Name"], options =>
    {
        options.Authority = $"https://{configuration["AuthNProvider:Domain"]}";
        options.ClientId = configuration["AuthNProvider:ClientId"];
        options.ClientSecret = configuration["AuthNProvider:ClientSecret"];
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.CallbackPath = configuration["AuthNProvider:CallbackPath"];
        options.SignedOutCallbackPath = configuration["AuthNProvider:SignedOutCallbackPath"];
        options.SignedOutRedirectUri = configuration["AuthNProvider:SignedOutRedirectUri"];
        options.ClaimsIssuer = configuration["AuthNProvider:Name"];

        options.Events = new OpenIdConnectEvents
        {
            OnRedirectToIdentityProviderForSignOut = (context) =>
            {
                var logoutUri = $"https://{configuration["AuthNProvider:Domain"]}{configuration["AuthNProvider:FederatedLogoutPartialUri"]}{configuration["AuthNProvider:ClientId"]}";

                var postLogoutUri = context.Properties.RedirectUri;
                if (!string.IsNullOrEmpty(postLogoutUri))
                {
                    if (postLogoutUri.StartsWith("/"))
                    {
                        var request = context.Request;
                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                    }
                    logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                }

                context.Response.Redirect(logoutUri);
                context.HandleResponse();

                return Task.CompletedTask;
            }
        };

        options.Events.OnSignedOutCallbackRedirect = (context) =>
        {
            context.Response.Redirect(options.SignedOutRedirectUri);
            context.HandleResponse();

            return Task.CompletedTask;
        };
    });

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddMudServices();
#if DEBUG
    builder.Services.AddDbContextFactory<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("AppDbContext"), options => options.EnableRetryOnFailure())
               .EnableSensitiveDataLogging());
#else
    builder.Services.AddDbContextFactory<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("AppDbContext"), options => options.EnableRetryOnFailure()));
#endif

    builder.Services.AddLocalization();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseRequestLocalization("en-CA");

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Default", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
#if DEBUG
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .WriteTo.Console()
#endif
        .WriteTo.Seq("http://localhost:5341")
        .CreateLogger();

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    Console.WriteLine(ex.Message);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
