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

        if ((price <= minRule.Price) && !minRule.AlreadyOnRule)
        {
            minRule.AlreadyOnRule = true;
            NotifyAll($"{minRule.Ticker} caiu abaixo de {minRule.Price}. Preço atual: {price}");
        }else if(price > minRule.Price){
            minRule.AlreadyOnRule = false;
        }
        if (price >= maxRule.Price && !maxRule.AlreadyOnRule)
        {
            maxRule.AlreadyOnRule = true;
            NotifyAll($"{maxRule.Ticker} subiu acima de {maxRule.Price}. Preço atual: {price}");
        }else if(price < maxRule.Price){
            maxRule.AlreadyOnRule = false;
        }
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
