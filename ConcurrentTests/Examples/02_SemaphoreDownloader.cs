
namespace ConcurrentTests.Examples;

internal class _02_SemaphoreDownloader : IExample
{
    

    public async Task DoAction()
    {

        SemaphoreSlim semaphore = new(3);

        Console.WriteLine("Download throttling (max 3 parallel): starting...");
        var tasks = Enumerable.Range(0, 10).Select(index =>
        {
            return Task.Run(async () =>
            {
                try {
                    await semaphore.WaitAsync();
                    Console.WriteLine($"Task {index}: started");
                    await Task.Delay(1000); // imitation of long operation
                    Console.WriteLine($"Task {index}: finished"); ;
                } 
                finally
                {
                    semaphore.Release();
                }
            });
        });

        await Task.WhenAll(tasks);
        Console.WriteLine("All downloads completed.");
    }
}
