using StockMonitor.Services;
using StockMonitor.Test.Core;

namespace StockMonitor.Test.ApiChecks;

public class HgBrasilHealthCheck : IApiHealthCheck
{
    public string Name => "HgBrasil";

    public async Task RunAsync()
    {
        try
        {
            var provider = new HgBrasilPriceProvider();
            var price = await provider.GetPriceAsync("PETR4");
            Console.WriteLine($"✅ {Name}: sucesso! Preço: {price}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ {Name}: falhou. \nErro: {ex.Message}");
        }
    }
}
