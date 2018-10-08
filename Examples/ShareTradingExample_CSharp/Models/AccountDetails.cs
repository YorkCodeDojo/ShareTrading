using System;
using System.Collections.Generic;

namespace ShareTradingAPI.ViewModels
{
    public class AccountDetails
    {
        public Guid AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int TotalFromTransactions { get; set; }
        public int OpeningCash { get; set; }
        public List<Investment> Portfolio { get; set; }
    }
}
