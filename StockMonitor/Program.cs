using Microsoft.Extensions.DependencyInjection;
using StockMonitor.Services;
using StockMonitor.Alerts;
using StockMonitor.Notifications;
using StockMonitor.Test.Core;
using StockMonitor.Test.ApiChecks;
using DotNetEnv;

Env.Load();

var services = new ServiceCollection();

// Providers
services.AddSingleton<BrapiPriceProvider>();
services.AddSingleton<HgBrasilPriceProvider>();
// Interface dos Providers
services.AddSingleton<IPriceProvider>(sp =>
{
    var brapi = sp.GetRequiredService<BrapiPriceProvider>();
    var hg = sp.GetRequiredService<HgBrasilPriceProvider>();
    return new ProviderFallbackService(brapi, hg);
});

// Notifiers (registrar múltiplos INotifier)
var emailConfig = new EmailConfig
{
    From = Env.GetString("MAIL_FROM") ?? "noreply@example.com",
    To = Env.GetString("MAIL_TO") ?? "noreply@example.com",
    SmtpHost = Env.GetString("SMTP_HOST") ?? "localhost",
    SmtpPort = int.TryParse(Env.GetString("SMTP_PORT"), out var port) ? port : 25,
    SmtpUser = Env.GetString("SMTP_USER"),
    SmtpPass = Env.GetString("SMTP_PASS"),
    EnableSsl = bool.TryParse(Env.GetString("SMTP_SSL"), out var ssl) && ssl
};

services.AddSingleton(emailConfig);
services.AddSingleton<IEmailClient, SmtpEmailClient>();
services.AddSingleton<INotifier, EmailNotifier>();


// Alert service
services.AddSingleton<PriceAlertService>();

// Testes
services.AddSingleton<IApiHealthCheck, BrapiHealthCheck>();
services.AddSingleton<IApiHealthCheck, HgBrasilHealthCheck>();
services.AddSingleton<TestRunner>();

var provider = services.BuildServiceProvider();

if (args.Length == 1 && args[0] == "--test")
{
    var tests = new List<IApiHealthCheck>
    {
        new BrapiHealthCheck(),
        new HgBrasilHealthCheck(),
    };

    var runner = new TestRunner(tests);
    await runner.RunAllAsync();
    return;
}

if (args.Length != 3)
{
    Console.WriteLine("Você digitou algo errado\nUso: \ndotnet run -- SIGLA PRECO_MIN PRECO_MAX\ndotnet run --test");
    return;
}

string ticker = args[0];
decimal min = decimal.Parse(args[1]);
decimal max = decimal.Parse(args[2]);

var alertService = provider.GetRequiredService<PriceAlertService>();

var minRule = new IAlertRule(ticker, min, isMinRule: true);
var maxRule = new IAlertRule(ticker, max, isMinRule: false);

await alertService.CheckAsync(minRule, maxRule);
