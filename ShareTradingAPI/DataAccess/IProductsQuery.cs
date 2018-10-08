using ShareTradingAPI.ViewModels;
using System.Threading.Tasks;

namespace ShareTradingAPI.DataAccess
{
    public interface IProductsQuery
    {
        Task<Product[]> Evaluate();
    }
}
