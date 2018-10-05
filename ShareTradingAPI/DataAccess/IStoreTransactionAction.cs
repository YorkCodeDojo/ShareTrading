using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess
{
    public interface IStoreTransactionAction
    {
        Task Execute(Transaction transaction);
    }
}