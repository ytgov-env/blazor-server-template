using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorServerTemplate.App.Authentication;

public class LogoutModel : PageModel
{
    private IConfiguration Configuration { get; }

    public LogoutModel(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public async Task OnGet()
    {
        await HttpContext.SignOutAsync(Configuration["AuthNProvider:Name"], new AuthenticationProperties { RedirectUri = Configuration["AuthNProvider:SignedOutCallbackPath"] });
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
