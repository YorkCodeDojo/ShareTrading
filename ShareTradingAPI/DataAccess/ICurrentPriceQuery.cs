using System.Threading.Tasks;

namespace ShareTradingAPI.DataAccess
{
    public interface ICurrentPriceQuery
    {
        Task<int> Evaluate(string productCode);
    }
}
