using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenGettingProducts : IClassFixture<WebApplicationFactory<Startup>>
    {
        readonly WebApplicationFactory<Startup> _factory;

        public WhenGettingProducts(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task The_Known_Products_Are_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var response = await client.GetAsync($"/api/Products");
            var products = await response.Content.ReadAsJsonAsync<List<Product>>();

            Assert.Equal(4, products.Count());
            Assert.Equal(Constants.ProductA, products[0].ProductCode); 
            Assert.Equal(Constants.ProductB, products[1].ProductCode); 
            Assert.Equal(Constants.ProductC, products[2].ProductCode);
            Assert.Equal(Constants.ProductD, products[3].ProductCode);
        }

    }
}
