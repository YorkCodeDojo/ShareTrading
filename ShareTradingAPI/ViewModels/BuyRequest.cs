using System;
using System.ComponentModel.DataAnnotations;

namespace ShareTradingAPI.ViewModels
{
    public class BuyRequest
    {
        public Guid AccountNumber { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int MaxCost { get; set; }
    }
}
