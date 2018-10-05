using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShareTradingAPI.DataAccess
{
    public interface IProductsQuery
    {
        Task<IEnumerable<string>> Evaluate();
    }
}
