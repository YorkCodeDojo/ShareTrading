using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess
{
    public interface ITransactionsForAccountQuery
    {
        Task<List<Transaction>> Evaluate(Guid accountNumber);
    }
}