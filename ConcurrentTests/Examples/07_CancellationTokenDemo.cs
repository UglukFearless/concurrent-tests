
namespace ConcurrentTests.Examples;

internal class _07_CancellationTokenDemo : IExample
{
    private const int Timeout = 2000;
    private const int MaxSemaphoreCount = 1;

    public async Task DoAction()
    {
        
        CancellationTokenSource tokenSource = new(Timeout);
        SemaphoreSlim semaphore = new(1);

        try
        {
            await AcquireSemaphoreAsync(semaphore, tokenSource.Token, "Task1");
            await AcquireSemaphoreAsync(semaphore, tokenSource.Token, "Task2");
        }
        finally
        {
            if (semaphore.CurrentCount > 0)
            {
                var toRelease = MaxSemaphoreCount - semaphore.CurrentCount;
                semaphore.Release(toRelease);
            }
                
            semaphore.Dispose();
        } 
    }

    private async Task AcquireSemaphoreAsync(SemaphoreSlim semaphore, CancellationToken token, string taskName)
    {
        try
        {
            await semaphore.WaitAsync(token);
            Console.WriteLine($"{taskName}: semaphore acquired");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"{taskName}: canceled by timeout.");
        }
        catch (Exception) 
        {
            if (semaphore.CurrentCount > 0)
                semaphore.Release();
        }
    }
}
