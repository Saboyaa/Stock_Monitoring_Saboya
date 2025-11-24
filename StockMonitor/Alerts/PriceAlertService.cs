using StockMonitor.Services;
using StockMonitor.Notifications;

namespace StockMonitor.Alerts;

public class PriceAlertService
{
    private readonly IPriceProvider _provider;
    private readonly IEnumerable<INotifier> _notifiers;

    public PriceAlertService(IPriceProvider provider, IEnumerable<INotifier> notifiers)
    {
        _provider = provider;
        _notifiers = notifiers;
    }

    public async Task CheckAsync(IAlertRule minRule, IAlertRule maxRule)
    {
        var price = await _provider.GetPriceAsync(minRule.Ticker);

        if (price <= minRule.Price)
            NotifyAll($"{minRule.Ticker} caiu abaixo de {minRule.Price}. Preço atual: {price}");

        if (price >= maxRule.Price)
            NotifyAll($"{maxRule.Ticker} subiu acima de {maxRule.Price}. Preço atual: {price}");
    }

    private void NotifyAll(string message)
    {
        foreach (var notifier in _notifiers)
        {
            try
            {
                notifier.Notify(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Notifier falhou: {ex.Message}");
            }
        }
    }
}
