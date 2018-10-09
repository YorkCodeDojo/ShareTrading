using System;
using System.ComponentModel.DataAnnotations;

namespace ShareTradingAPI.ViewModels
{
    public class BuyRequest
    {
        public Guid AccountNumber { get; set; }

        public string ProductCode { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Most you are willing to pay
        /// </summary>
        [Range(0, int.MaxValue)]
        public int MaxUnitPrice { get; set; }
    }
}
