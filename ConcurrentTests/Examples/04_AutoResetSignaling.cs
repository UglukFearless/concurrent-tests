namespace ConcurrentTests.Examples;

internal class _04_AutoResetSignaling : IExample
{
    public async Task DoAction()
    {
        await DoWithAutoEvent();
        Console.WriteLine();
        await DoWithManualEvent();
    }

    private async Task DoWithAutoEvent()
    {
        Console.WriteLine("AutoResetEvent demo: each Set() releases exactly one waiting task");
        using (AutoResetEvent autoReset = new(false))
        {
            var tasks = Enumerable.Range(0, 3).Select((index) => Task.Run(() =>
            {
                Console.WriteLine($"Task {index} with auto event is starting...");
                autoReset.WaitOne();
                Console.WriteLine($"Task {index} with auto event is completed...");
            })).ToArray();

            var eventTask = Task.Run(async () =>
            {
                while (!tasks.All(t => t.IsCompleted))
                {
                    await Task.Delay(500);
                    Console.WriteLine("Set the event!");
                    autoReset.Set();
                }
            });

            await Task.WhenAll(tasks);
            await eventTask;

            Console.WriteLine("Auto event is completed.");
        }
    }

    private async Task DoWithManualEvent()
    {
        Console.WriteLine("ManualResetEvent demo: first Set() releases all waiting tasks");
        using (ManualResetEvent manualReset = new(false))
        {
            var tasks = Enumerable.Range(0, 3).Select((index) => Task.Run(() =>
            {
                Console.WriteLine($"Task {index} with manual event is starting...");
                manualReset.WaitOne();
                Console.WriteLine($"Task {index} with manual event is completed...");
            })).ToArray();

            var eventTask = Task.Run(async () =>
            {
                while (!tasks.All(t => t.IsCompleted))
                {
                    await Task.Delay(500);
                    Console.WriteLine("Set the event!");
                    manualReset.Set();
                }
            });

            await Task.WhenAll(tasks);
            await eventTask;

            Console.WriteLine("Manual event is completed.");
        }
    }
}
