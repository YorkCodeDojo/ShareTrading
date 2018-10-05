using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess
{
    public interface ICreateAccountAction
    {
        Task Execute(AccountDetails account);
    }
}
