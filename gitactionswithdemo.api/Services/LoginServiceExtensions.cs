using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class LoginExtensions
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Auth").WithTags("Auth");

        // Simple login endpoint for demo purposes. In production use a user store and hashed passwords.
        group.MapPost("/login", ([FromBody] LoginRequest req, IConfiguration config) =>
        {
            // demo credentials
            if (req is null || req.Username != "demo" || req.Password != "P@ssw0rd")
            {
                return Results.Unauthorized();
            }

            var jwtSection = config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key is required");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expireMinutes = jwtSection.GetValue<int>("ExpireMinutes");

            var claims = new[] { new Claim(ClaimTypes.Name, req.Username), new Claim(ClaimTypes.Role, "User") };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { access_token = tokenString, token_type = "Bearer", expires_in = expireMinutes * 60 });
        });
    }

    public record LoginRequest(string Username, string Password);
}
