namespace StockMonitor.Notifications;

public class EmailNotifier : INotifier
{
    private readonly IEmailClient _client;
    private readonly EmailConfig _config;

    public EmailNotifier(IEmailClient client, EmailConfig config)
    {
        _client = client;
        _config = config;
    }

    public void Notify(string message)
    {
        try
        {
            _client.Send(_config.From, _config.To, "StockMonitor - Alerta de preço", message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN] EmailNotifier falhou: {ex.Message}");
        }
    }
}
