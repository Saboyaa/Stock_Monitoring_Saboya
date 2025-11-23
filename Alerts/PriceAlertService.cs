using StockMonitor.Services;
using StockMonitor.Notifications;

namespace StockMonitor.Alerts;

public class PriceAlertService
{
    private readonly IPriceProvider _provider;
    private readonly INotifier _notifier;

    public PriceAlertService(IPriceProvider provider, INotifier notifier)
    {
        _provider = provider;
        _notifier = notifier;
    }

    public async Task CheckAsync(AlertRule minRule, AlertRule maxRule)
    {
        var price = await _provider.GetPriceAsync(minRule.Ticker);

        if (price <= minRule.Price)
            _notifier.Notify($"{minRule.Ticker} caiu abaixo de {minRule.Price}. Preço atual: {price}");

        if (price >= maxRule.Price)
            _notifier.Notify($"{maxRule.Ticker} subiu acima de {maxRule.Price}. Preço atual: {price}");
    }
}
