using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace gitactionswithdemo.razor.Pages;

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EditModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public ProductDto Product { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ProductsApi");
        try
        {
            var p = await client.GetFromJsonAsync<ProductDto>($"/api/Product/{id}");
            if (p == null) return NotFound();
            Product = p;
            return Page();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load product: {ex.Message}");
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpClientFactory.CreateClient("ProductsApi");
        try
        {
            var resp = await client.PutAsJsonAsync($"/api/Product/{Product.Id}", Product);
            if (resp.IsSuccessStatusCode) return RedirectToPage("/Index");
            ModelState.AddModelError(string.Empty, $"API Error: {resp.StatusCode}");
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Request failed: {ex.Message}");
            return Page();
        }
    }
}
