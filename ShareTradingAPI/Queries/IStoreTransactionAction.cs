using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.Queries
{
    public interface IStoreTransactionAction
    {
        Task Execute(Transaction transaction);
    }
}