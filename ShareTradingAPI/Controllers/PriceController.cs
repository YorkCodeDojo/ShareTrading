using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.Queries;
using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        readonly ICurrentPriceQuery _currentPriceQuery;

        public PriceController(ICurrentPriceQuery currentPriceQuery)
        {
            _currentPriceQuery = currentPriceQuery;
        }

        public async Task<ActionResult<Price>> Get()
        {
            var currentPrice = await _currentPriceQuery.Evaluate();
            return new Price()
            {
                Time = DateTime.Now,
                UnitCost = currentPrice
            };
        }
    }
}