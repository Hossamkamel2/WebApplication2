namespace WebApplication2.Persistence.Models
{
    public class StockData
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public long Volume { get; set; }
    }
}
