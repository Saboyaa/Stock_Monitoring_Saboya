namespace StockMonitor.Alerts;

public class AlertRule
{
    public string Ticker { get; }
    public decimal Price { get; }
    public bool IsMinRule { get; } // true = alerta abaixo, false = alerta acima

    public AlertRule(string ticker, decimal price, bool isMinRule)
    {
        Ticker = ticker;
        Price = price;
        IsMinRule = isMinRule;
    }
}
