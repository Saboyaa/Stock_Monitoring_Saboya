using StockMonitor.Services;
using Xunit;

public class FallbackProviderTests
{
    [Fact]
    public async Task ShouldReturnFromSecondProviderWhenFirstFails()
    {
        var failing = new FailingProvider();
        var success = new FakeProvider(42);

        var fallback = new ProviderFallbackService(failing, success);

        var result = await fallback.GetPriceAsync("TESTE");

        Assert.Equal(42, result);
    }

    [Fact]
    public async Task ShouldThrowWhenAllProvidersFail()
    {
        var p1 = new FailingProvider();
        var p2 = new FailingProvider();
        var fallback = new ProviderFallbackService(p1, p2);
        await Assert.ThrowsAsync<Exception>(() =>
            fallback.GetPriceAsync("T")
        );
    }

    private class FailingProvider : IPriceProvider
    {
        public Task<decimal> GetPriceAsync(string ticker)
            => throw new Exception("fail");
    }

    private class FakeProvider : IPriceProvider
    {
        private readonly decimal _price;
        public FakeProvider(decimal price) => _price = price;
        public Task<decimal> GetPriceAsync(string ticker)
            => Task.FromResult(_price);
    }
}
