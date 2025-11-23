namespace StockMonitor.Notifications;

public interface INotifier
{
    void Notify(string message);
}
