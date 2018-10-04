using System.Threading.Tasks;

namespace ShareTradingAPI.Queries
{
    public interface ICurrentPriceQuery
    {
        Task<int> Evaluate();
    }
}