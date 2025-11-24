namespace StockMonitor.Services;

public interface IPriceProvider
{
    Task<decimal> GetPriceAsync(string ticker);
}
