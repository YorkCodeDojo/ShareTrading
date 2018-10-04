using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.Queries
{
    public interface IAccountQuery
    {
        Task<AccountDetails> Evaluate(Guid accountNumber);
    }
}