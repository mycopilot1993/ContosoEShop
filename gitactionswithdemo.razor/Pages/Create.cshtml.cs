using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace gitactionswithdemo.razor.Pages;

public class CreateModel : PageModel
{
	private readonly IHttpClientFactory _httpClientFactory;

	public CreateModel(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	[BindProperty]
	public ProductDto Product { get; set; } = new();

	public void OnGet()
	{
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var client = _httpClientFactory.CreateClient("ProductsApi");
		try
		{
			var resp = await client.PostAsJsonAsync("/api/Product/", Product);
			if (resp.IsSuccessStatusCode)
			{
				return RedirectToPage("/Index");
			}

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

