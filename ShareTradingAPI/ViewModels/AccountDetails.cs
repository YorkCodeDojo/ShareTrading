using System;
using System.Collections.Generic;

namespace ShareTradingAPI.ViewModels
{
    public class AccountDetails
    {
        public Guid AccountNumber { get; set; }
        public string AccountName { get; set; }

        /// <summary>
        /// Total money spent on / profit made from shares
        /// </summary>
        public int TotalFromTransactions { get; set; }
        public int OpeningCash { get; set; }

        /// <summary>
        /// Shares you currently own
        /// </summary>
        public List<Investment> Portfolio { get; set; }
    }
}
