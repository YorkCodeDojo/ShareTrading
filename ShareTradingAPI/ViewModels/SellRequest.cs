using System;
using System.ComponentModel.DataAnnotations;

namespace ShareTradingAPI.ViewModels
{
    public class SellRequest
    {
        public Guid AccountNumber { get; set; }

        public string ProductCode { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Smallest value you are willing to sell for
        /// </summary>
        [Range(0, int.MaxValue)]
        public int MinUnitPrice { get; set; }
    }
}