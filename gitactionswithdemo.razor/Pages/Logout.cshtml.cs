using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace gitactionswithdemo.razor.Pages;

public class LogoutModel : PageModel
{
    public async Task OnGet()
    {
        Response.Cookies.Delete("id_token");
        await HttpContext.SignOutAsync("Cookies");
        Response.Redirect("/");
    }
}
