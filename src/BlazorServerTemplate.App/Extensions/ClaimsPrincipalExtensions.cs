using System.Security.Claims;

namespace BlazorServerTemplate.App.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetFormattedName(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirst("name")?.Value.Replace('.', ' ') ?? "";
}
