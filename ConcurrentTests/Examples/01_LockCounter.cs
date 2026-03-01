
namespace ConcurrentTests.Examples;

internal class _01_LockCounter
{
    private int _unsafeCounter = 0;
    private int _lockCounter = 0;

    private object _lock = new();

    public async Task DoAction()
    {
        try
        {
            await DoUnsafe();
            await DoSafe();    
        }
        catch (Exception ex) 
        { 
            Console.Error.WriteLine(ex.ToString()); // First failed task exception
        }
        
    }

    private async Task DoUnsafe()
    {
        Console.WriteLine("Unsafe counter (without lock): starting...");
        var unsafeTasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                _unsafeCounter++;
            }
        }));
        await Task.WhenAll(unsafeTasks);
        Console.WriteLine($"Result: {_unsafeCounter}");
    }

    private async Task DoSafe()
    {
        Console.WriteLine("Safe counter (with lock): starting...");
        var safeTasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                lock (_lock)
                {
                    _lockCounter++;
                }
            }
        }));
        await Task.WhenAll(safeTasks);
        Console.WriteLine($"Result: {_lockCounter}");
    }
}
