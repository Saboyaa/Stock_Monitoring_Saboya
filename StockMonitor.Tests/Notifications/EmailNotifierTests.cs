using Moq;
using StockMonitor.Notifications;
using Xunit;

public class EmailNotifierTests
{
    [Fact]
    public void Notify_CallsEmailClient()
    {
        var mockClient = new Mock<IEmailClient>();
        var config = new EmailConfig { From = "a@b.com", To = "c@d.com" };
        var notifier = new EmailNotifier(mockClient.Object, config);

        notifier.Notify("Teste");

        mockClient.Verify(c => c.Send("a@b.com", "c@d.com", 
            "StockMonitor - Alerta de preço", "Teste"), Times.Once);
    }
}
