using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.DataAccess;
using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly ICurrentPriceQuery _currentPriceQuery;
        readonly IStoreTransactionAction _storeTransactionAction;

        public PurchasesController(IAccountQuery accountQuery, ICurrentPriceQuery currentPriceQuery, IStoreTransactionAction storeTransactionAction)
        {
            _accountQuery = accountQuery;
            _currentPriceQuery = currentPriceQuery;
            _storeTransactionAction = storeTransactionAction;
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> Buy([FromBody] BuyRequest buyRequest)
        {
            var account = await _accountQuery.Evaluate(buyRequest.AccountNumber);
            var currentPrice = await _currentPriceQuery.Evaluate(buyRequest.ProductCode);

            if (currentPrice > buyRequest.MaxCost)
            {
                return new Purchase()
                {
                    Message = $"The current price of {currentPrice} is higher then the maximum you specified of {buyRequest.MaxCost}.",
                    Success = false,
                };
            }

            var availableCash = account.OpeningCash + account.TotalFromTransactions;
            var totalCost = currentPrice * buyRequest.Quantity;

            if (totalCost > availableCash)
            {
                return new Purchase()
                {
                    Message = $"The trade will cost {totalCost} but you only have {availableCash}.",
                    Success = false,
                };
            }

            var transaction = new Transaction()
            {
                AccountNumber = account.AccountNumber,
                Quantity = buyRequest.Quantity,
                Time = DateTime.Now,
                UnitPrice = currentPrice,
                TotalValue = -totalCost,
                ProductCode = buyRequest.ProductCode,
                ID = Guid.NewGuid()
            };
            await _storeTransactionAction.Execute(transaction);

            return new Purchase()
            {
                Message = "Success",
                Quantity = buyRequest.Quantity,
                ProductCode = buyRequest.ProductCode,
                Success = true,
                TotalValue = totalCost,
                UnitPrice = currentPrice,
                TransactionID = transaction.ID
            };
        }

    }
}