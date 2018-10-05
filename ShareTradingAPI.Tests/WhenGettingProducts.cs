using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
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
        public async Task The_Three_Known_Products_Are_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var response = await client.GetAsync($"/api/Products");
            var products = await response.Content.ReadAsJsonAsync<List<string>>();

            Assert.Equal(Constants.ProductA, products[0]); 
            Assert.Equal(Constants.ProductB, products[1]); 
            Assert.Equal(Constants.ProductC, products[2]);
        }

    }
}
