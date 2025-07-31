using CityFm.API.Policies;
using CityFm.API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CityFM API", Version = "v1" });
});


builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IRateLimiter>(_ => new RateLimiter(5, TimeSpan.FromSeconds(60)));
builder.Services.AddScoped<IProductService, ProductService>();


var config = builder.Configuration;
var apiKey = config["Vendor:XApiKey"];
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Warning: Vendor:XApiKey is not configured.");
    Console.ResetColor();
}


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

