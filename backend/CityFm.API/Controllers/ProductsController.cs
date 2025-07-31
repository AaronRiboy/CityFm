using CityFm.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityFm.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    // Get the products
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        try
        {
            var result = await _service.GetProductsAsync(ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(429, new { error = ex.Message });
        }
    }
}