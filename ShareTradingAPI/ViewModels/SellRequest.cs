using System;

namespace ShareTradingAPI.ViewModels
{
    public class SellRequest
    {
        public Guid AccountNumber { get; set; }
        public int Quantity { get; set; }
        public int MinCost { get; set; }
    }
}
