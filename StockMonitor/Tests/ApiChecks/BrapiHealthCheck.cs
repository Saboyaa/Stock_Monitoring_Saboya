using StockMonitor.Services;
using StockMonitor.Test.Core;

namespace StockMonitor.Test.ApiChecks;

public class BrapiHealthCheck : IApiHealthCheck
{
    public string Name => "BRAPI";

    public async Task RunAsync()
    {
        try
        {
            var provider = new BrapiPriceProvider();
            var price = await provider.GetPriceAsync("PETR4");
            Console.WriteLine($"✅ {Name}: sucesso! Preço: {price}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ {Name}: falhou. \nErro: {ex.Message}");
        }
    }
}
