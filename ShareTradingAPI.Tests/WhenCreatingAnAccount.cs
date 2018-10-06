using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ShareTradingAPI.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace ShareTradingAPI.Tests
{
    public class WhenCreatingAnAccount : IClassFixture<WebApplicationFactory<Startup>>
    {

        readonly WebApplicationFactory<Startup> _factory;

        public WhenCreatingAnAccount(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task The_Account_Number_Is_Generated()
        {
            var client = _factory.CreateDefaultClient();

            var requestData = new NewAccountRequest() { AccountName = "Test Account - " + Guid.NewGuid().ToString() };

            var response = await client.PostAsJsonAsync("/api/Accounts", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.NotEqual(Guid.Empty, responseData.AccountNumber);
        }

        [Fact]
        public async Task The_Account_Starts_With_The_Correct_Opening_Balance()
        {
            var client = _factory.CreateDefaultClient();

            var requestData = new NewAccountRequest() { AccountName = "Test Account - " + Guid.NewGuid().ToString() };

            var response = await client.PostAsJsonAsync("/api/Accounts", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Equal(Constants.OpeningBalance, responseData.OpeningCash);
        }

        [Fact]
        public async Task The_Account_Has_No_Transactions()
        {
            var client = _factory.CreateDefaultClient();

            var requestData = new NewAccountRequest() { AccountName = "Test Account - " + Guid.NewGuid().ToString() };

            var response = await client.PostAsJsonAsync("/api/Accounts", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Empty(responseData.Portfolio);
        }

        [Fact]
        public async Task The_Account_Has_No_Money_From_Transactions()
        {
            var client = _factory.CreateDefaultClient();

            var requestData = new NewAccountRequest() { AccountName = "Test Account - " + Guid.NewGuid().ToString() };

            var response = await client.PostAsJsonAsync("/api/Accounts", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Assert.Equal(0, responseData.TotalFromTransactions);
        }

    }
}
