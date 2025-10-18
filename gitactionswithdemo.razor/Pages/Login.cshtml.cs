using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace gitactionswithdemo.razor.Pages;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Enter username and password.";
            return Page();
        }

        var client = _httpClientFactory.CreateClient("AuthApi");
        try
        {
            var resp = await client.PostAsJsonAsync("/api/Auth/login", new { Username, Password });
            if (!resp.IsSuccessStatusCode)
            {
                ErrorMessage = "Invalid credentials";
                return Page();
            }

            var payload = await resp.Content.ReadFromJsonAsync<LoginResponse>();
            if (payload == null || string.IsNullOrEmpty(payload.access_token))
            {
                ErrorMessage = "Login failed";
                return Page();
            }

            // store token in secure cookie (HttpOnly)
            Response.Cookies.Append("id_token", payload.access_token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddSeconds(payload.expires_in)
            });

            // sign-in user locally using cookie auth for UI
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, Username)
            };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "Cookies");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("Cookies", principal);

            return LocalRedirect("/");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }

    private record LoginResponse(string access_token, string token_type, int expires_in);
}
