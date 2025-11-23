using StockMonitor.Test.Core;

namespace StockMonitor.Test.Core;

public class TestRunner
{
    private readonly List<IApiHealthCheck> _tests;

    public TestRunner(List<IApiHealthCheck> tests)
    {
        _tests = tests;
    }

    public async Task RunAllAsync()
    {
        foreach (var test in _tests)
        {
            Console.WriteLine($"Executando teste: {test.Name}");
            try
            {
                await test.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {test.Name}: falhou. Erro: {ex.Message}");
            }
        }
    }
}
