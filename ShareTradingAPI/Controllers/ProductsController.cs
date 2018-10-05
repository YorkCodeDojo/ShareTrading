using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.DataAccess;
using ShareTradingAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly ICurrentPriceQuery _currentPriceQuery;
        readonly IProductsQuery _productsQuery;

        public ProductsController(ICurrentPriceQuery currentPriceQuery, IProductsQuery productsQuery)
        {
            _currentPriceQuery = currentPriceQuery;
            _productsQuery = productsQuery;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return (await _productsQuery.Evaluate()).ToArray();
        }

        [HttpGet("{ProductCode}")]
        public async Task<ActionResult<Product>> GetProduct(string ProductCode)
        {
            var currentPrice = await _currentPriceQuery.Evaluate(ProductCode, 0);
            if (currentPrice == DataAccess.SQLServer.CurrentPriceQuery.ErrorConditions.ProductDoesNotExist) return NotFound("Product does not exist");
            if (currentPrice == DataAccess.SQLServer.CurrentPriceQuery.ErrorConditions.PriceDoesNotExist) return BadRequest("No valid price");

            return new Product()
            {
                Time = DateTime.Now,
                CurrentUnitCost = currentPrice,
                ProductCode = ProductCode
            };
        }
    }
}