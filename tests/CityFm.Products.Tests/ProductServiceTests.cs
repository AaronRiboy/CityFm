using Xunit;
using Moq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using CityFm.API.Services;
using CityFm.API.Policies;
using System.Threading.Tasks;
using System.Threading;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductsAsync_ShouldThrow_WhenApiKeyMissing()
    {
        // Arrange
        var mockHttpFactory = new Mock<IHttpClientFactory>();
        var mockLogger = new Mock<ILogger<ProductService>>();
        var mockRateLimiter = new Mock<IRateLimiter>();
        var mockMemoryCache = new MemoryCache(new MemoryCacheOptions());
        
        // Empty configuration - no API key
        var mockConfig = new ConfigurationBuilder().Build(); 

        mockRateLimiter.Setup(x => x.TryEnter()).Returns(true);

        var service = new ProductService(
            mockHttpFactory.Object,
            mockConfig,
            mockRateLimiter.Object,
            mockMemoryCache,
            mockLogger.Object
        );

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.GetProductsAsync(CancellationToken.None));
        Assert.Equal("API key not configured.", ex.Message);
    }
}