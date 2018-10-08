using System;

namespace ShareTradingAPI.ViewModels
{
    public class Sale
    {
        public bool Success { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalValue { get; set; }
        public string Message { get; set; }
        public string ProductCode { get; set; }
        public Guid TransactionID { get; set; }
    }
}
