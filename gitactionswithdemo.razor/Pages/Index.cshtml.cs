using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace gitactionswithdemo.razor.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<ProductDto> Products { get; set; } = new();

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("ProductsApi");
        try
        {
            var list = await client.GetFromJsonAsync<List<ProductDto>>("/api/Product/");
            if (list is not null)
            {
                Products = list;
            }
        }
        catch (Exception ex)
        {
            // Log or handle as needed. For now just leave Products empty.
            Console.WriteLine($"Failed to load products from API: {ex.Message}");
        }
    }
    public async Task<IActionResult> PostProducts([FromBody] ProductDto newProduct)
    {
        var client = _httpClientFactory.CreateClient("ProductsApi");
        try
        {
            var response = await client.PostAsJsonAsync("/api/Product/", newProduct);
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            if (createdProduct is not null)
            {
                return RedirectToPage(); // Refresh the page to show the new product
            }
        }
        catch (Exception ex)
        {
            // Log or handle as needed. For now just leave Products empty.
            Console.WriteLine($"Failed to load products from API: {ex.Message}");
        }
        return Page();
    }
}
