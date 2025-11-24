namespace StockMonitor.Services;

public class ProviderFallbackService : IPriceProvider
{
    private readonly List<IPriceProvider> _providers;

    public ProviderFallbackService(params IPriceProvider[] providers)
    {
        _providers = providers.ToList();
    }

    public async Task<decimal> GetPriceAsync(string ticker)
    {
        foreach (var provider in _providers)
        {
            try
            {
                var price = await provider.GetPriceAsync(ticker);

                if (price > 0)
                    return price;
            }
            catch
            {
                // ignoramos e tentamos o próximo
            }
        }

        throw new Exception("Nenhuma API conseguiu retornar o preço.");
    }
}
