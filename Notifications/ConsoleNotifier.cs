namespace StockMonitor.Notifications;

public class ConsoleNotifier : INotifier
{
    public void Notify(string message)
    {
        Console.WriteLine($"[ALERTA] {message}");
    }
}
