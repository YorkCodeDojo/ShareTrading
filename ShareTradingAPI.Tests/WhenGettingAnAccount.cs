using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenGettingAnAccount : IClassFixture<WebApplicationFactory<Startup>>
    {
        readonly WebApplicationFactory<Startup> _factory;

        public WhenGettingAnAccount(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Which_Does_Not_Exist_Then_A_404_Error_Will_Be_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var madeUpAccountNumber = Guid.NewGuid().ToString();

            var response = await client.GetAsync("/api/Accounts/" + madeUpAccountNumber);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task Which_Does_Exist_Then_Its_Details_Will_Be_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            var response = await client.GetAsync("/api/Accounts/" + createdAccountDetails.AccountNumber);
            var gotAccountDetails = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Equal(createdAccountDetails.AccountName, gotAccountDetails.AccountName);
            Assert.Equal(createdAccountDetails.AccountNumber, gotAccountDetails.AccountNumber);
            Assert.Equal(0, gotAccountDetails.TotalFromTransactions);
            Assert.Equal(Constants.OpeningBalance, gotAccountDetails.OpeningCash);
            Assert.Empty(gotAccountDetails.Portfolio);
        }


        [Fact]
        public async Task Which_Has_A_Single_Purchase_The_Total_From_Transactions_Will_Be_Negative()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);

            var response = await client.GetAsync("/api/Accounts/" + createdAccountDetails.AccountNumber);
            var gotAccountDetails = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Equal(createdAccountDetails.AccountName, gotAccountDetails.AccountName);
            Assert.Equal(createdAccountDetails.AccountNumber, gotAccountDetails.AccountNumber);
            Assert.Equal(-purchase.TotalValue, gotAccountDetails.TotalFromTransactions);
            Assert.Equal(Constants.OpeningBalance, gotAccountDetails.OpeningCash);
            Assert.Single(gotAccountDetails.Portfolio);
        }

        [Fact]
        public async Task Which_Has_A_Single_Purchase_And_Sale_The_Total_From_Transactions_Will_Be_Negative()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);
            var sale = await CreateSale(client, createdAccountDetails);

            var response = await client.GetAsync("/api/Accounts/" + createdAccountDetails.AccountNumber);
            var gotAccountDetails = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Equal(createdAccountDetails.AccountName, gotAccountDetails.AccountName);
            Assert.Equal(createdAccountDetails.AccountNumber, gotAccountDetails.AccountNumber);
            Assert.Equal(-(purchase.TotalValue - sale.TotalValue), gotAccountDetails.TotalFromTransactions);
            Assert.Equal(Constants.OpeningBalance, gotAccountDetails.OpeningCash);
            Assert.Single(gotAccountDetails.Portfolio);
        }


        [Fact]
        public async Task Which_Has_A_Single_Purchase_The_Portfolio_Will_Contain_A_Single_Product()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);

            var response = await client.GetAsync("/api/Accounts/" + createdAccountDetails.AccountNumber);
            var gotAccountDetails = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Single(gotAccountDetails.Portfolio);
            var product = gotAccountDetails.Portfolio.First();
            Assert.Equal(Constants.ProductA, product.ProductCode);
            Assert.Equal(Constants.QuantityPurchased, product.Quantity);
        }

        [Fact]
        public async Task Which_Has_A_Single_Purchase_And_Sale_The_Portfolio_Will_Contain_A_Single_Product()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);
            var purchase = await CreatePurchase(client, createdAccountDetails);
            var sale = await CreateSale(client, createdAccountDetails);

            var response = await client.GetAsync("/api/Accounts/" + createdAccountDetails.AccountNumber);
            var gotAccountDetails = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Single(gotAccountDetails.Portfolio);
            var product = gotAccountDetails.Portfolio.First();
            Assert.Equal(Constants.ProductA, product.ProductCode);
            Assert.Equal(Constants.QuantityPurchased - Constants.QuantitySold, product.Quantity);
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
