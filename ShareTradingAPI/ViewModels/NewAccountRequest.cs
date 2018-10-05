using System.ComponentModel.DataAnnotations;

namespace ShareTradingAPI.ViewModels
{
    public class NewAccountRequest
    {
        [MaxLength(100)]
        public string AccountName { get; set; }
    }
}
