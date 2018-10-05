using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Net;
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
