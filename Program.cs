using StockMonitor.Services;
using StockMonitor.Alerts;
using StockMonitor.Notifications;
using StockMonitor.Test.Core;
using StockMonitor.Test.ApiChecks;
using DotNetEnv;

Env.Load();
if (args.Length == 1 && args[0] == "--test")
{
    var runner = new TestRunner(new List<IApiHealthCheck>
    {
        new BrapiHealthCheck(),
        new HgBrasilHealthCheck(),
    });

    await runner.RunAllAsync();
    return;
}

if (args.Length != 3)
{
    Console.WriteLine("Uso: dotnet run -- SIGLA PRECO_MIN PRECO_MAX");
    return;
}

string ticker = args[0];
decimal min = decimal.Parse(args[1]);
decimal max = decimal.Parse(args[2]);

var provider = new ProviderFallbackService(
    new BrapiPriceProvider(),
    new HgBrasilPriceProvider()
);

var notifier = new ConsoleNotifier();
var alertService = new PriceAlertService(provider, notifier);

var minRule = new AlertRule(ticker, min, isMinRule: true);
var maxRule = new AlertRule(ticker, max, isMinRule: false);

await alertService.CheckAsync(minRule, maxRule);
