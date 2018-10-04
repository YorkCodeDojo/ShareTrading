using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.Queries;
using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly ICurrentPriceQuery _currentPriceQuery;
        readonly IStoreTransactionAction _storeTransactionAction;
        readonly IUpdateAccountAction _updateAccountAction;

        public SellController(IAccountQuery accountQuery, ICurrentPriceQuery currentPriceQuery, IUpdateAccountAction updateAccountAction, IStoreTransactionAction storeTransactionAction)
       {
            _accountQuery = accountQuery;
            _currentPriceQuery = currentPriceQuery;
            _updateAccountAction = updateAccountAction;
            _storeTransactionAction = storeTransactionAction;
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> Sell([FromBody] SellRequest sellRequest)
        {
            var account = await _accountQuery.Evaluate(sellRequest.AccountNumber);
            var currentPrice = await _currentPriceQuery.Evaluate();

            if (currentPrice < sellRequest.MinCost)
            {
                return new Sale()
                {
                    Message = $"The current price of {currentPrice} is lower than the minimum you specified of {sellRequest.MinCost}.",
                    Success = false,
                };
            }

            var totalCost = currentPrice * sellRequest.Quantity;

            if (sellRequest.Quantity > account.SharesHeld)
            {
                return new Sale()
                {
                    Message = $"You have requested to sell {sellRequest.Quantity} but you only have {account.SharesHeld}.",
                    Success = false,
                };
            }

            account.Cash += totalCost;
            account.SharesHeld -= sellRequest.Quantity;
            await _updateAccountAction.Execute(account);

            var transaction = new Transaction()
            {
                AccountNumber = account.AccountNumber,
                Quantity = -sellRequest.Quantity,
                Time = DateTime.Now,
                UnitCost = currentPrice,
                TotalCost = totalCost
            };
            await _storeTransactionAction.Execute(transaction);

            return new Sale()
            {
                Message = "Success",
                Quantity = sellRequest.Quantity,
                Success = true,
                TotalCost = totalCost,
                UnitCost = currentPrice
            };
        }

    }
}