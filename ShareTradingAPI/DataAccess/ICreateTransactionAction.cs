using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess
{
    public interface ICreateTransactionAction
    {
        Task Execute(Transaction transaction);
    }
}