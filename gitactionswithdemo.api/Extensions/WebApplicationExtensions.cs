using Microsoft.AspNetCore.Builder;

namespace gitactionswithdemo.api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Configure environment-specific middleware (production safety features).
    /// This keeps Program.cs lean and centralizes the logic for reuse.
    /// </summary>
    public static WebApplication UseEnvironmentConfiguration(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        return app;
    }
}
