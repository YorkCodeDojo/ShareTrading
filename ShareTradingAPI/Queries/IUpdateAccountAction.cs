using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.Queries
{
    public interface IUpdateAccountAction
    {
        Task Execute(AccountDetails account);
    }
}
