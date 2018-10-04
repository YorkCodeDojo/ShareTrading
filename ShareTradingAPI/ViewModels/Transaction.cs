using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTradingAPI.ViewModels
{
    public class Transaction
    {
        public Guid AccountNumber { get; internal set; }
        public int Quantity { get; internal set; }
        public object Time { get; internal set; }
        public int UnitCost { get; internal set; }
        public int TotalCost { get; internal set; }
    }
}
