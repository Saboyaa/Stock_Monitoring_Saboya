using System.Net.Http.Json;

namespace StockMonitor.Services;

public class BrapiPriceProvider : IPriceProvider
{
    private readonly HttpClient _http = new();

    public async Task<decimal> GetPriceAsync(string ticker)
    {
        var url = $"https://brapi.dev/api/quote/{ticker}?range=1d&interval=1d";
        var result = await _http.GetFromJsonAsync<BrapiResponse>(url);
        return result?.Results?.FirstOrDefault()?.RegularMarketPrice ?? 0m;
    }
    private class BrapiResponse
    {
        public List<Result>? Results { get; set; }
    }
    private class Result
    {
        public decimal RegularMarketPrice { get; set; }
    }
}
