using StockMonitor.Alerts;
using StockMonitor.Services;
using StockMonitor.Notifications;
using Xunit;

public class AlertServiceTests
{
    [Fact]
    public async Task ShouldNotifyWhenPriceIsBelowMin()
    {
        var fakeProvider = new FakeProvider(5); // preço fixo
        var fakeNotifier = new FakeNotifier();

        var service = new PriceAlertService(fakeProvider, new[] { fakeNotifier });

        var minRule = new AlertRule("TESTE", 10, isMinRule: true);
        var maxRule = new AlertRule("TESTE", 100, isMinRule: false);

        await service.CheckAsync(minRule, maxRule);

        Assert.Contains("caiu abaixo", fakeNotifier.Messages[0]);
    }

    private class FakeProvider : IPriceProvider
    {
        private readonly decimal _price;
        public FakeProvider(decimal price) => _price = price;
        public Task<decimal> GetPriceAsync(string ticker) => Task.FromResult(_price);
    }

    private class FakeNotifier : INotifier
    {
        public List<string> Messages { get; } = new();
        public void Notify(string message) => Messages.Add(message);
    }
}