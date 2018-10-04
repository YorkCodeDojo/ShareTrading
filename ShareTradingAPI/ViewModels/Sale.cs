namespace ShareTradingAPI.ViewModels
{
    public class Sale
    {
        public bool Success { get; set; }
        public int Quantity { get; set; }
        public int UnitCost { get; set; }
        public int TotalCost { get; set; }
        public string Message { get; set; }
    }
}
