using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ShareTradingAPI.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenGettingThePriceForProductD : IClassFixture<WebApplicationFactory<Startup>>
    {
        readonly WebApplicationFactory<Startup> _factory;

        public WhenGettingThePriceForProductD(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task The_Price_Is_Varies_In_A_Known_Way()
        {
            var controlledTime = new ControlledTime();

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<Pricing.ITimeCalculator>(_ => controlledTime);
                });
            })
            .CreateClient();

            var startTime = new DateTime(2018, 1, 1, 18, 30, 0);
            var endTime = new DateTime(2018, 1, 1, 21, 0, 0);
            var currentTime = startTime;

            var outputFile = @"c:\temp\ProductD.csv";
            if (File.Exists(outputFile)) File.Delete(outputFile);
            File.AppendAllText(outputFile, "Time,Price" + System.Environment.NewLine);

            while (currentTime <= endTime)
            {
                controlledTime.TimeInSeconds = (currentTime - startTime).TotalSeconds;
                var response = await client.GetAsync($"/api/Products/ProductD");
                var price = await response.Content.ReadAsJsonAsync<Price>();

                File.AppendAllText(outputFile, currentTime + "," + price.CurrentUnitCost + System.Environment.NewLine);
              //  Assert.Equal(123, price.CurrentUnitCost);
                currentTime = currentTime.AddSeconds(1);
            }

        }

        class ControlledTime : Pricing.ITimeCalculator
        {
            public double TimeInSeconds { get; set; }
            public double Evaluate() => TimeInSeconds;
        }

    }
}
