namespace StockMonitor.Test.Core;

public interface IApiHealthCheck
{
    string Name { get; }
    Task RunAsync();
}
