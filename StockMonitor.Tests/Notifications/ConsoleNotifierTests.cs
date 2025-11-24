using StockMonitor.Notifications;

namespace StockMonitor.Tests.Notifications;

public class ConsoleNotifierTests
{
    [Fact]
    public void WritesToConsole()
    {
        var notifier = new ConsoleNotifier();

        using var sw = new StringWriter();
        Console.SetOut(sw);

        notifier.Notify("teste");

        var output = sw.ToString();
        Assert.Contains("teste", output);
        Assert.Contains("[ALERTA]", output);
    }
}
