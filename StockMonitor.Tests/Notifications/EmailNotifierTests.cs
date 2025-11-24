using StockMonitor.Notifications;
using Xunit;

public class EmailNotifierTests
{
    [Fact]
    public void Notify_ShouldNotThrow()
    {
        var notifier = new EmailNotifier();
        notifier.Notify("teste");
    }
}
