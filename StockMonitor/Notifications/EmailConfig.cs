namespace StockMonitor.Notifications;

public class EmailConfig
{
    public string From { get; init; } = "noreply@example.com";
    public string To { get; init; } = "noreply@example.com";
    public string SmtpHost { get; init; } = "localhost";
    public int SmtpPort { get; init; } = 25;
    public string? SmtpUser { get; init; }
    public string? SmtpPass { get; init; }
    public bool EnableSsl { get; init; } = false;
}
