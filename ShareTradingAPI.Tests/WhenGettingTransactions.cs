using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenGettingTransactions : IClassFixture<WebApplicationFactory<Startup>>
    {
        readonly WebApplicationFactory<Startup> _factory;

        public WhenGettingTransactions(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Given_A_New_Account_Then_No_Data_Will_Be_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            var response = await client.GetAsync($"/api/Accounts/{createdAccountDetails.AccountNumber}/Transactions");
            var transactions = await response.Content.ReadAsJsonAsync<IEnumerable<Transaction>>();

            Assert.Empty(transactions);
        }

        [Fact]
        public async Task Given_A_New_Account_With_One_Purchase_Then_One_Transaction_Will_Be_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            await CreatePurchase(client, createdAccountDetails);

            var response = await client.GetAsync($"/api/Accounts/{createdAccountDetails.AccountNumber}/Transactions");
            var transactions = await response.Content.ReadAsJsonAsync<IEnumerable<Transaction>>();

            Assert.Single(transactions);
            Assert.Equal(createdAccountDetails.AccountNumber, transactions.First().AccountNumber);
            Assert.NotEqual(Guid.Empty, transactions.First().ID);
            Assert.Equal(Constants.ProductA, transactions.First().ProductCode);
            Assert.Equal(10, transactions.First().Quantity);
            Assert.True(transactions.First().TotalValue == -transactions.First().Quantity * transactions.First().UnitPrice);
            Assert.True(transactions.First().UnitPrice > 0);
        }

        [Fact]
        public async Task Given_A_New_Account_With_One_Purchase_And_One_Sale_Then_Two_Transactions_Will_Be_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            await CreatePurchase(client, createdAccountDetails);
            await CreateSale(client, createdAccountDetails);

            var response = await client.GetAsync($"/api/Accounts/{createdAccountDetails.AccountNumber}/Transactions");
            var transactions = await response.Content.ReadAsJsonAsync<IEnumerable<Transaction>>();

            Assert.Equal(2, transactions.Count());

            var purchaseTransaction = transactions.First();
            var saleTransaction = transactions.Last();

            Assert.Equal(createdAccountDetails.AccountNumber, saleTransaction.AccountNumber);
            Assert.NotEqual(Guid.Empty, saleTransaction.ID);
            Assert.Equal(Constants.ProductA, saleTransaction.ProductCode);
            Assert.Equal(Constants.QuantitySold, -saleTransaction.Quantity);
            Assert.True(saleTransaction.TotalValue == -saleTransaction.Quantity * saleTransaction.UnitPrice);
            Assert.True(saleTransaction.UnitPrice > 0);
        }

        private static async Task<Purchase> CreatePurchase(HttpClient client, AccountDetails createdAccountDetails)
        {
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
            return responseData;
        }

        private static async Task<AccountDetails> CreateAccount(HttpClient client)
        {
            var requestData = new NewAccountRequest() { AccountName = "Test Account - " + Guid.NewGuid().ToString() };
            var response = await client.PostAsJsonAsync("/api/Accounts", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();
            return responseData;
        }

        private static async Task<Sale> CreateSale(HttpClient client, AccountDetails createdAccountDetails)
        {
            var requestData = new SellRequest()
            {
                AccountNumber = createdAccountDetails.AccountNumber,
                MinUnitPrice = 100,
                ProductCode = Constants.ProductA,
                Quantity = Constants.QuantitySold
            };

            var response = await client.PostAsJsonAsync("/api/Sales", requestData);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Sale>();
            Assert.True(responseData.Success);
            return responseData;
        }
    }
}
