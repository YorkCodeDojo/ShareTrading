using System.ComponentModel.DataAnnotations;

namespace ShareTradingAPI.ViewModels
{
    public class NewAccountRequest
    {

        /// <summary>
        /// Pick a nice unique name for your team
        /// </summary>
        [MaxLength(100)]
        public string AccountName { get; set; }
    }
}
