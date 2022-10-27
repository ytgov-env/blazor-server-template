using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorServerTemplate.App.Authentication;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public LoginModel(IConfiguration configuration) => Configuration = configuration;

    private IConfiguration Configuration { get; }

    public async Task OnGet(string redirectUri) =>
        await HttpContext.ChallengeAsync(Configuration["AuthNProvider:Name"],
            new AuthenticationProperties { RedirectUri = redirectUri });
}
