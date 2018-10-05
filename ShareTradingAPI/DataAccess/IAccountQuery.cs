using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.DataAccess
{
    public interface IAccountQuery
    {
        Task<AccountDetails> Evaluate(Guid accountNumber);
    }
}