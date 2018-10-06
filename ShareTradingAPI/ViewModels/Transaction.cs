using System;

namespace ShareTradingAPI.ViewModels
{
    public class Transaction
    {
        public Guid ID { get;  set; }
        public Guid AccountNumber { get;  set; }
        public int Quantity { get;  set; }
        public DateTime Time { get;  set; }
        public int UnitPrice { get;  set; }
        public int TotalValue { get;  set; }
        public string ProductCode { get;  set; }
    }
}
