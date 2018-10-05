using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.DataAccess;
using ShareTradingAPI.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly ICurrentPriceQuery _currentPriceQuery;
        readonly IStoreTransactionAction _storeTransactionAction;

        public SalesController(IAccountQuery accountQuery, ICurrentPriceQuery currentPriceQuery, IStoreTransactionAction storeTransactionAction)
       {
            _accountQuery = accountQuery;
            _currentPriceQuery = currentPriceQuery;
            _storeTransactionAction = storeTransactionAction;
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> Sell([FromBody] SellRequest sellRequest)
        {
            var account = await _accountQuery.Evaluate(sellRequest.AccountNumber);
            var currentPrice = await _currentPriceQuery.Evaluate(sellRequest.ProductCode);

            if (currentPrice < sellRequest.MinCost)
            {
                return new Sale()
                {
                    Message = $"The current price of {currentPrice} is lower than the minimum you specified of {sellRequest.MinCost}.",
                    Success = false,
                };
            }

            var totalIncome = currentPrice * sellRequest.Quantity;

            var sharesCurrentlyHeld = account.Portfolio.FirstOrDefault(investment => investment.ProductCode == sellRequest.ProductCode)?.Quantity ?? 0;

            if (sellRequest.Quantity > sharesCurrentlyHeld)
            {
                return new Sale()
                {
                    Message = $"You have requested to sell {sellRequest.Quantity} but you only have {sharesCurrentlyHeld}.",
                    Success = false,
                };
            }

            var transaction = new Transaction()
            {
                AccountNumber = account.AccountNumber,
                Quantity = -sellRequest.Quantity,
                Time = DateTime.Now,
                UnitPrice = currentPrice,
                TotalValue = totalIncome,
                ProductCode = sellRequest.ProductCode,
                ID = Guid.NewGuid()
            };
            await _storeTransactionAction.Execute(transaction);

            return new Sale()
            {
                Message = "Success",
                Quantity = sellRequest.Quantity,
                ProductCode = sellRequest.ProductCode,
                Success = true,
                TotalValue = totalIncome,
                UnitPrice = currentPrice,
                TransactionID = transaction.ID
            };
        }

    }
}