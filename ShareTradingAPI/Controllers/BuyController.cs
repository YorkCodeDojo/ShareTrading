using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.DataAccess;
using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly ICurrentPriceQuery _currentPriceQuery;
        readonly ICreateOrUpdateAccountAction _updateAccountAction;
        readonly IStoreTransactionAction _storeTransactionAction;

        public BuyController(IAccountQuery accountQuery, ICurrentPriceQuery currentPriceQuery, ICreateOrUpdateAccountAction updateAccountAction, IStoreTransactionAction storeTransactionAction)
        {
            _accountQuery = accountQuery;
            _currentPriceQuery = currentPriceQuery;
            _updateAccountAction = updateAccountAction;
            _storeTransactionAction = storeTransactionAction;
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> Buy([FromBody] BuyRequest buyRequest)
        {
            var account = await _accountQuery.Evaluate(buyRequest.AccountNumber);
            var currentPrice = await _currentPriceQuery.Evaluate();

            if (currentPrice > buyRequest.MaxCost)
            {
                return new Purchase()
                {
                    Message = $"The current price of {currentPrice} is higher then the maximum you specified of {buyRequest.MaxCost}.",
                    Success = false,
                };
            }

            var totalCost = currentPrice * buyRequest.Quantity;

            if (totalCost > account.Cash)
            {
                return new Purchase()
                {
                    Message = $"The trade will cost {totalCost} but you only have {account.Cash}.",
                    Success = false,
                };
            }

            account.Cash -= totalCost;
            account.SharesHeld += buyRequest.Quantity;
            await _updateAccountAction.Execute(account);

            var transaction = new Transaction()
            {
                AccountNumber = account.AccountNumber,
                Quantity = buyRequest.Quantity,
                Time = DateTime.Now,
                UnitCost = currentPrice,
                TotalCost = totalCost
            };
            await _storeTransactionAction.Execute(transaction);

            return new Purchase()
            {
                Message = "Success",
                Quantity = buyRequest.Quantity,
                Success = true,
                TotalCost = totalCost,
                UnitCost = currentPrice
            };
        }

    }
}