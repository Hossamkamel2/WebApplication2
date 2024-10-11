using Newtonsoft.Json;
using System.Net.Http;
using WebApplication2.Persistence.Models;

namespace WebApplication2.PolygonIntegration
{
    public class PolygonClient
    {
        private readonly HttpClient _httpClient;

        public PolygonClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<StockData> GetStockDataAsync(string ticker)
        {
            string url = $"https://api.polygon.io/v1/open-close/{ticker}/{DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd")}?adjusted=true&apiKey=ExxZEMqdN85GDc7CM9XMpUJGlGt_QFBg";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API request failed with status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<dynamic>(content);

            return new StockData
            {
                Ticker = ticker,
                Date = DateTime.Parse(jsonData["from"].ToString()),
                OpenPrice = (decimal)jsonData["open"],
                ClosePrice = (decimal)jsonData["close"],
                HighPrice = (decimal)jsonData["high"],
                LowPrice = (decimal)jsonData["low"],
                Volume = (long)jsonData["volume"]
            };
        }
    }
}
