public static class ServiceExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Authentication: use cookies for the UI; JWT is stored in a secure cookie for API calls
        services.AddAuthentication("Cookies")
        .AddCookie("Cookies", options =>
        {
            options.LoginPath = "/Login";
            options.LogoutPath = "/Logout";
        });
        var apiBase = configuration["ApiBaseUrl"] ?? "https://localhost:7029";
        services.AddHttpClient("AuthApi", client => client.BaseAddress = new Uri(apiBase));
        services.AddHttpClient("ProductsApi", client => client.BaseAddress = new Uri(apiBase))
            .AddHttpMessageHandler<TokenDelegatingHandler>();

        return services;
    }
}