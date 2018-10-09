using System;

namespace ShareTradingAPI.ViewModels
{
    public class Transaction
    {
        public Guid ID { get;  set; }
        public Guid AccountNumber { get;  set; }

        /// <summary>
        /// +ve for a purchase,  and -ve for a sale
        /// </summary>
        public int Quantity { get;  set; }
        public DateTime Time { get;  set; }
        public int UnitPrice { get;  set; }

        /// <summary>
        /// +ve for a sale and -ve for a purchase
        /// </summary>
        public int TotalValue { get;  set; }
        public string ProductCode { get;  set; }
    }
}
