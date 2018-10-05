﻿using Microsoft.AspNetCore.Mvc.Testing;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
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
        public async Task For_A_New_Account_Then_No_Data_Will_Be_Returned()
        {
            var client = _factory.CreateDefaultClient();

            var createdAccountDetails = await CreateAccount(client);

            var response = await client.GetAsync($"/api/Accounts/{createdAccountDetails.AccountNumber}/Transactions");
            var transactions = await response.Content.ReadAsJsonAsync<IEnumerable<Transaction>>();

            Assert.Empty(transactions);

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