using ShareTradingAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShareTradingAPI.DataAccess
{
    public interface IAccountByNameQuery
    {
        Task<AccountDetails> Evaluate(string accountName);
    }
}