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

        /// <summary>
        /// Gets a list of all the available products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return (await _productsQuery.Evaluate());
        }


        /// <summary>
        /// Gets the current price of a product
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <returns></returns>
        [HttpGet("{ProductCode}")]
        public async Task<ActionResult<Price>> GetProduct(string ProductCode)
        {
            var currentPrice = await _currentPriceQuery.Evaluate(ProductCode);
            if (currentPrice == DataAccess.CurrentPriceQuery.ErrorConditions.ProductDoesNotExist) return NotFound("Product does not exist");
            if (currentPrice == DataAccess.CurrentPriceQuery.ErrorConditions.PriceDoesNotExist) return BadRequest("No valid price");

            return new Price()
            {
                Time = DateTime.Now,
                CurrentUnitCost = currentPrice,
                ProductCode = ProductCode
            };
        }
    }
}