using Microsoft.EntityFrameworkCore;
using BlazorServerTemplate.Data;
using BlazorServerTemplate.Data.Entities.Users;

namespace BlazorServerTemplate.App;

public class AppParameters
{
    public IDbContextFactory<AppDbContext> ContextFactory { get; set; } = null!;

    public UserSettings UserSettings { get; init; } = new();
    public string UserEmail { get; init; } = string.Empty;
    public int UserId { get; init; }

    public async Task UpdateSettings()
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        var user = await context.Users.FindAsync(UserId);
        if (user != null)
        {
            user.Settings = UserSettings;

            await context.SaveChangesAsync();
        }
    }
}
