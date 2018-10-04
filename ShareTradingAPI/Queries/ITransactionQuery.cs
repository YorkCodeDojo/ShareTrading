using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.Queries
{
    public interface ITransactionQuery
    {
        Task<ActionResult<IEnumerable<Transaction>>> Evaluate(Guid accountNumber);
    }
}