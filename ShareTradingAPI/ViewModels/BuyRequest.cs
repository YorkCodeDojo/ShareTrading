using System;

namespace ShareTradingAPI.ViewModels
{
    public class BuyRequest
    {
        public Guid AccountNumber { get; set; }
        public int Quantity { get; set; }
        public int MaxCost { get; set; }
    }
}
