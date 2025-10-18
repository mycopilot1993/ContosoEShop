using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public static class Dependencies
{
    public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //JWT authentication
        var jwtSection = configuration.GetSection("Jwt");
        var jwtKey = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key is required in configuration");
        var issuer = jwtSection.GetValue<string>("Issuer");
        var audience = jwtSection.GetValue<string>("Audience");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = !string.IsNullOrEmpty(issuer),
                    ValidIssuer = issuer,
                    ValidateAudience = !string.IsNullOrEmpty(audience),
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true
                };
            });


        return services;
    }
    public static IServiceCollection AddProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("ProductDbContext")
            ?? throw new InvalidOperationException("Connection string 'ProductDbContext' not found.");
        services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
    public static IServiceCollection AddSeedData(this IServiceCollection services)
    {
        // Seed data: register seeder and startup filter so seeding runs during application startup
        services.AddSingleton<IDataSeeder, DataSeeder>();
        //services.AddSingleton<IStartupFilter, SeedStartupFilter>();
        return services;
    }
}