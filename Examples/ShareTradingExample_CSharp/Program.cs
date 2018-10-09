using ShareTradingAPI.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShareTradingExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://sharetradingapi.azurewebsites.net");


            const string account_name = "David Betteridge";
            var account_code = await RegisterAccount(httpClient, account_name);

            await DisplayAccountDetails(httpClient, account_code);

            await DisplayAvailableProducts(httpClient);

            await DisplayProduct(httpClient, "ProductC");

            await MakePurchase(httpClient, account_code, "ProductC", 8000, 1);

            await DisplayAccountDetails(httpClient, account_code);

            await MakeSale(httpClient, account_code, "ProductC", 6000, 1);

            await DisplayAccountDetails(httpClient, account_code);

            await DisplayTransactionsForAccount(httpClient, account_code);

            Console.ReadKey();
        }

        private static async Task<Sale> MakeSale(HttpClient httpClient, Guid account_code, string productCode, int minUnitPrice, int quantity)
        {
            var requestData = new SellRequest ()
            {
                AccountNumber = account_code,
                MinUnitPrice = minUnitPrice,
                ProductCode = productCode,
                Quantity = quantity
            };

            var response = await httpClient.PostAsJsonAsync("/api/Sales", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Sale>();

            //TODO: Check the sales was successful

            return responseData;
        }

        private static async Task<Purchase> MakePurchase(HttpClient httpClient, Guid account_code, string productCode, int maxUnitPrice, int quantity)
        {
            var requestData = new BuyRequest()
            {
                AccountNumber = account_code,
                MaxUnitPrice = maxUnitPrice,
                ProductCode = productCode,
                Quantity = quantity
            };

            var response = await httpClient.PostAsJsonAsync("/api/Purchases", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<Purchase>();

            //TODO: Check the purchase was successful

            return responseData;
        }

        private static async Task DisplayProduct(HttpClient httpClient, string productCode)
        {
            var response = await httpClient.GetAsync("/api/Products/" + productCode);
            var product = await response.Content.ReadAsJsonAsync<Price>();

            Console.WriteLine("");
            Console.WriteLine("Product");
            Console.WriteLine("-------");
            Console.WriteLine($"ProductCode : {product.ProductCode}");
            Console.WriteLine($"CurrentUnitCost : {product.CurrentUnitCost}");
            Console.WriteLine($"Time : {product.Time}");

        }

        private static async Task DisplayAvailableProducts(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync("/api/Products");
            var products = await response.Content.ReadAsJsonAsync<Product[]>();

            Console.WriteLine("");
            Console.WriteLine("Available Products");
            Console.WriteLine("------------------");
            foreach (var item in products)
            {
                Console.WriteLine($"{item.ProductCode} : {item.Description}");
            }
        }

        private static async Task DisplayTransactionsForAccount(HttpClient httpClient, Guid account_code)
        {
            var response = await httpClient.GetAsync("/api/Accounts/" + account_code.ToString() + "/Transactions");

            var transactions = await response.Content.ReadAsJsonAsync<Transaction[]>();

            Console.WriteLine("");
            Console.WriteLine("Transactions for Account");
            Console.WriteLine("-------------------------");
            foreach (var item in transactions)
            {
                Console.WriteLine($"{item.ID} : {item.ProductCode} {item.Quantity} {item.UnitPrice} {item.TotalValue} {item.Time}");
            }
        }

        private static async Task DisplayAccountDetails(HttpClient httpClient, Guid account_code)
        {
            var response = await httpClient.GetAsync("/api/Accounts/" + account_code.ToString());
            var accountDetails = await response.Content.ReadAsJsonAsync<AccountDetails>();

            Console.WriteLine("");
            Console.WriteLine("Account Details");
            Console.WriteLine("---------------");
            Console.WriteLine($"Account Name : {accountDetails.AccountName}");
            Console.WriteLine($"Account Number : {accountDetails.AccountNumber}");
            Console.WriteLine($"TotalFromTransactions : {accountDetails.TotalFromTransactions}");
            Console.WriteLine($"OpeningBalance : {accountDetails.OpeningCash}");
            Console.WriteLine("Portfolio:");
            foreach (var item in accountDetails.Portfolio)
            {
                Console.WriteLine($"{item.ProductCode} : {item.Quantity}");
            }
        }

        private static async Task<Guid> RegisterAccount(HttpClient httpClient, string account_name)
        {
            var requestData = new NewAccountRequest() { AccountName = account_name };

            var response = await httpClient.PostAsJsonAsync("/api/Accounts", requestData);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsJsonAsync<AccountDetails>();

            return responseData.AccountNumber;
        }
    }
}
