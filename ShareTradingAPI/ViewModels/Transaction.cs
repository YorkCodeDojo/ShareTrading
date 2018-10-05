using System;

namespace ShareTradingAPI.ViewModels
{
    public class Transaction
    {
        public Guid ID { get; internal set; }
        public Guid AccountNumber { get; internal set; }
        public int Quantity { get; internal set; }
        public DateTime Time { get; internal set; }
        public int UnitPrice { get; internal set; }
        public int TotalValue { get; internal set; }
        public string ProductCode { get; internal set; }
    }
}
