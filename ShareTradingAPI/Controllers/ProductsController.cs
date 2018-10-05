using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.DataAccess;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly ICurrentPriceQuery _currentPriceQuery;

        public ProductsController(ICurrentPriceQuery currentPriceQuery)
        {
            _currentPriceQuery = currentPriceQuery;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return new string[1] { "Dojo" };
        }

        [HttpGet("{ProductCode}")]
        public async Task<ActionResult<Product>> GetProduct(string ProductCode)
        {
            var currentPrice = await _currentPriceQuery.Evaluate(ProductCode);
            return new Product()
            {
                Time = DateTime.Now,
                CurrentUnitCost = currentPrice,
                ProductCode = ProductCode
            };
        }
    }
}