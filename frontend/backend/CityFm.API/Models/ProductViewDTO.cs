namespace CityFm.API.Models;

public class ProductViewModel
{
    public string ProductCode { get; set; } = default!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public decimal MarkedUpPrice { get; set; }
}