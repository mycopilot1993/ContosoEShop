using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace gitactionswithdemo.razor.Pages;

public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DeleteModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public ProductDto? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ProductsApi");
        try
        {
            Product = await client.GetFromJsonAsync<ProductDto>($"/api/Product/{id}");
            if (Product == null) return NotFound();
            return Page();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load product: {ex.Message}");
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ProductsApi");
        try
        {
            var resp = await client.DeleteAsync($"/api/Product/{id}");
            if (resp.IsSuccessStatusCode) return RedirectToPage("/Index");
            return StatusCode((int)resp.StatusCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete failed: {ex.Message}");
            return StatusCode(500);
        }
    }
}
