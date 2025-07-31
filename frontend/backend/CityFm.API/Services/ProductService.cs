using CityFm.API.Models;
using CityFm.API.Policies;
using Microsoft.Extensions.Caching.Memory;

namespace CityFm.API.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly IRateLimiter _rateLimiter;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IHttpClientFactory httpClientFactory, IConfiguration config,
        IRateLimiter rateLimiter, IMemoryCache cache, ILogger<ProductService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _rateLimiter = rateLimiter;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<ProductViewModel>> GetProductsAsync(CancellationToken ct)
    {
        if (_cache.TryGetValue("product_list", out List<ProductViewModel> cached))
            return cached;

        // Rate limiter check
        if (!_rateLimiter.TryEnter())
            throw new Exception("Rate limit exceeded. Try again later.");

        // API Key check
        var apiKey = _config["Vendor:XApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("API key not configured.");

        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://myfm-dev.cityfm.com.au/xapi/api/product/get-product-list");
        request.Headers.Add("X-Api-Key", apiKey);

        var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        //check response 
        var rawJson = await response.Content.ReadAsStringAsync(ct);
        _logger.LogInformation("Vendor response: {json}", rawJson);

        var vendorData = System.Text.Json.JsonSerializer.Deserialize<List<VendorProductDTO>>(rawJson, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var result = vendorData?.Select(p => new ProductViewModel
        {
            ProductCode = p.ProductCode,
            Description = p.Description,
            BasePrice = p.Price,
            //markerd up price is 20% more than base price
            MarkedUpPrice = Math.Round(p.Price * 1.2m, 2)
        }).ToList() ?? new();

        _cache.Set("product_list", result, TimeSpan.FromSeconds(60));

        return result;
    }
}
