using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess
{
    public interface ICreateOrUpdateAccountAction
    {
        Task Execute(AccountDetails account);
    }
}
