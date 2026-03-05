using System.Diagnostics;

namespace ConcurrentTests.Examples;

internal class _06_InterlockedCounter : IExample
{
    private int _lockCounter = 0;
    private int _interlockedCounter = 0;
    private readonly object _lock = new();

    public async Task DoAction()
    {
        var lockTask = DoWithLock();
        var interlockedTask = DoWithInterlocked();

        await lockTask;
        await interlockedTask;

        Console.WriteLine($"\nFinal results:");
        Console.WriteLine($"  Lock: {_lockCounter}");
        Console.WriteLine($"  Interlocked: {_interlockedCounter}");
    }

    private async Task DoWithLock()
    {
        Console.WriteLine("Counter with lock started.");
        Stopwatch sw = Stopwatch.StartNew();
        var tasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
        {
            for (var i = 0; i < 10000; i++)
            {
                lock (_lock)
                {
                    _lockCounter++;
                }
                
            }
        })).ToArray();
        await Task.WhenAll(tasks);
        sw.Stop();
        Console.WriteLine($"Time with lock: {sw.ElapsedMilliseconds} ms");
    }

    private async Task DoWithInterlocked()
    {
        Console.WriteLine("Counter with interlocked started.");
        Stopwatch sw = Stopwatch.StartNew();
        var tasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
        {
            for (var i = 0; i < 10000; i++)
            {
                Interlocked.Increment(ref _interlockedCounter);
            }
        })).ToArray();
        await Task.WhenAll(tasks);
        sw.Stop();
        Console.WriteLine($"Time with interlocked: {sw.ElapsedMilliseconds} ms");
    }
}
