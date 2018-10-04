using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTradingAPI.ViewModels
{
    public class AccountDetails
    {
        public Guid AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int SharesHeld { get; set; }
        public int Cash { get; set; }

    }
}
