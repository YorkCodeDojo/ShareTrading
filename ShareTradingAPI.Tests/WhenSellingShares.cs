using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenSellingShares : IClassFixture<WebApplicationFactory<Startup>>
    {
        readonly WebApplicationFactory<Startup> _factory;

        public WhenSellingShares(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task A_Transaction_Will_Be_Created()
        {
            const int Quantity_To_Sell = 8;

            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);

            var requestData = new SellRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MinUnitPrice = 100,
                ProductCode = Constants.ProductA,
                Quantity = Quantity_To_Sell
            };

            var response = await client.PostAsJsonAsync("/api/Sales", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Sale>();

            Assert.True(responseData.Success);
            Assert.Equal(Constants.ProductA, responseData.ProductCode);
            Assert.Equal(Quantity_To_Sell, responseData.Quantity);
            Assert.True(responseData.UnitPrice >= requestData.MinUnitPrice);
            Assert.Equal(responseData.UnitPrice * Quantity_To_Sell, responseData.TotalValue);
            Assert.NotEqual(Guid.Empty, responseData.TransactionID);
        }

        [Fact]
        public async Task A_Transaction_Will_Not_Be_Created_If_The_Price_Is_Too_Low()
        {
            const int Quantity_To_Sell = 8;

            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);

            var requestData = new SellRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MinUnitPrice = 1000,
                ProductCode = Constants.ProductA,
                Quantity = Quantity_To_Sell
            };

            var response = await client.PostAsJsonAsync("/api/Sales", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Sale>();

            Assert.False(responseData.Success);
            Assert.Equal(Constants.ProductA, responseData.ProductCode);
            Assert.Equal(0, responseData.Quantity);
            Assert.True(responseData.UnitPrice < requestData.MinUnitPrice);
            Assert.Equal(0, responseData.TotalValue);
            Assert.Equal(Guid.Empty, responseData.TransactionID);
        }

        [Fact]
        public async Task A_Transaction_Will_Not_Be_Created_If_You_Dont_Have_Enough_Shares()
        {
            const int Quantity_To_Sell = 20;

            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);

            var requestData = new SellRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MinUnitPrice = 1000,
                ProductCode = Constants.ProductA,
                Quantity = Quantity_To_Sell
            };

            var response = await client.PostAsJsonAsync("/api/Sales", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Sale>();

            Assert.False(responseData.Success);
            Assert.Equal(Constants.ProductA, responseData.ProductCode);
            Assert.Equal(0, responseData.Quantity);
            Assert.True(responseData.UnitPrice < requestData.MinUnitPrice);
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

        private static async Task<Purchase> CreatePurchase(HttpClient client, AccountDetails createdAccountDetails)
        {
            var requestData = new BuyRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MaxUnitPrice = 150,
                ProductCode = Constants.ProductA,
                Quantity = Constants.QuantityPurchased
            };

            var response = await client.PostAsJsonAsync("/api/Purchases", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Purchase>();
            Assert.True(responseData.Success);
            return responseData;
        }
    }
}
