namespace StockMonitor.Alerts;

public class IAlertRule
{
    public string Ticker { get; }
    public decimal Price { get; }
    public bool IsMinRule { get; } // true = alerta abaixo, false = alerta acima

    public IAlertRule(string ticker, decimal price, bool isMinRule)
    {
        Ticker = ticker;
        Price = price;
        IsMinRule = isMinRule;
    }
}
