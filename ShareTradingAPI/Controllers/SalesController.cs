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
        readonly ICreateTransactionAction _storeTransactionAction;

        public SalesController(IAccountQuery accountQuery, ICurrentPriceQuery currentPriceQuery, ICreateTransactionAction storeTransactionAction)
        {
            _accountQuery = accountQuery;
            _currentPriceQuery = currentPriceQuery;
            _storeTransactionAction = storeTransactionAction;
        }


        /// <summary>
        /// Sells shares in a product,  check the returned transaction to see if sale was successfull.
        /// </summary>
        /// <param name="sellRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Sale>> Sell([FromBody] SellRequest sellRequest)
        {
            var account = await _accountQuery.Evaluate(sellRequest.AccountNumber);
            if (account == null) return NotFound("Account does not exist");

            var currentPrice = await _currentPriceQuery.Evaluate(sellRequest.ProductCode);
            if (currentPrice == DataAccess.CurrentPriceQuery.ErrorConditions.ProductDoesNotExist) return NotFound("Product does not exist");
            if (currentPrice == DataAccess.CurrentPriceQuery.ErrorConditions.PriceDoesNotExist) return BadRequest("No valid price");

            if (currentPrice < sellRequest.MinUnitPrice)
            {
                return new Sale()
                {
                    UnitPrice = currentPrice,
                    Message = $"The current price of {currentPrice} is lower than the minimum you specified of {sellRequest.MinUnitPrice}.",
                    Success = false,
                    ProductCode = sellRequest.ProductCode,
                };
            }

            var totalIncome = currentPrice * sellRequest.Quantity;

            var sharesCurrentlyHeld = account.Portfolio.FirstOrDefault(investment => investment.ProductCode == sellRequest.ProductCode)?.Quantity ?? 0;

            if (sellRequest.Quantity > sharesCurrentlyHeld)
            {
                return new Sale()
                {
                    UnitPrice = currentPrice,
                    Message = $"You have requested to sell {sellRequest.Quantity} but you only have {sharesCurrentlyHeld}.",
                    Success = false,
                    ProductCode = sellRequest.ProductCode,
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