namespace StockMonitor.Notifications;

public interface IEmailClient
{
    void Send(string from, string to, string subject, string body);
}
