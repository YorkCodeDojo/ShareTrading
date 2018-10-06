using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenBuyingShares : IClassFixture<WebApplicationFactory<Startup>>
    {
        readonly WebApplicationFactory<Startup> _factory;

        public WhenBuyingShares(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task A_Transaction_Will_Be_Created()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            var requestData = new BuyRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MaxUnitPrice = 150,
                ProductCode = Constants.ProductA,
                Quantity = 10
            };

            var response = await client.PostAsJsonAsync("/api/Purchases", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Purchase>();

            Assert.True(responseData.Success);

        }

        [Fact]
        public async Task A_Transaction_Will_Not_Be_Created_If_The_Accounts_Does_Not_Have_Enough_Money()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            var requestData = new BuyRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MaxUnitPrice = 50,
                ProductCode = Constants.ProductA,
                Quantity = 10000
            };

            var response = await client.PostAsJsonAsync("/api/Purchases", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Purchase>();

            Assert.False(responseData.Success);
            Assert.Equal(Constants.ProductA, responseData.ProductCode);
            Assert.Equal(0, responseData.Quantity);
            Assert.Equal(0, responseData.TotalValue);
            Assert.Equal(Guid.Empty, responseData.TransactionID);
        }


        [Fact]
        public async Task A_Transaction_Will_Not_Be_Created_If_The_Price_Is_High()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            var requestData = new BuyRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MaxUnitPrice = 50,
                ProductCode = Constants.ProductA,
                Quantity = 10
            };

            var response = await client.PostAsJsonAsync("/api/Purchases", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Purchase>();

            Assert.False(responseData.Success);
            Assert.Equal(Constants.ProductA, responseData.ProductCode);
            Assert.Equal(0, responseData.Quantity);
            Assert.True(responseData.UnitPrice > requestData.MaxUnitPrice);
            Assert.Equal(0, responseData.TotalValue);
            Assert.Equal(Guid.Empty, responseData.TransactionID);
        }

        private static async Task<AccountDetails> CreateAccount(System.Net.Http.HttpClient client)
        {
            var requestData = new NewAccountRequest() { AccountName = "Test Account - " + Guid.NewGuid().ToString() };
            var response = await client.PostAsJsonAsync("/api/Accounts", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();
            return responseData;
        }
    }
}
