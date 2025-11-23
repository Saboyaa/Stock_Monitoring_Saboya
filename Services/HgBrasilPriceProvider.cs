using System.Net.Http.Json;
using System.Text.Json;

namespace StockMonitor.Services;

public class HgBrasilPriceProvider : IPriceProvider
{
    private readonly HttpClient _http = new();

    public async Task<decimal> GetPriceAsync(string ticker)
    {
        string apiKey = Environment.GetEnvironmentVariable("HG_API_KEY") ?? "";
        var url = $"https://api.hgbrasil.com/finance/stock_price?symbol={ticker}&key={apiKey}";

        using var response = await _http.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("results", out var results) &&
            results.TryGetProperty("error", out var errorProp) &&
            errorProp.GetBoolean())
        {
            var message = results.GetProperty("message").GetString();
            throw new InvalidOperationException($"API HgBrasil retornou erro: {message}");
        }

        var stock = results.GetProperty(ticker.ToUpper());
        var price = stock.GetProperty("price").GetDecimal();
        return price;
    }
}
