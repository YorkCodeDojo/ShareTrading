using System;

namespace ShareTradingAPI.ViewModels
{
    public class Price
    {
        public string ProductCode { get; set; }
        public int CurrentUnitCost { get; set; }
        public DateTime Time { get; set; }
    }
}
