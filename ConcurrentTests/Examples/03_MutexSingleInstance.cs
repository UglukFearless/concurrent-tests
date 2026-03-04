namespace ConcurrentTests.Examples;

internal class _03_MutexSingleInstance : IExample
{
    private const string MutexName = "Global\\ConcurrentTestsMutex";
    public async Task DoAction()
    {
        using Mutex mutex = new(true, MutexName, out bool createdNew);

        if (!createdNew)
        {
            Console.WriteLine($"Application already running (mutex '{MutexName}' is held by another process).");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        try
        {
            Console.WriteLine("Application started successfully (mutex acquired).");
            Console.WriteLine("Press any key to exit and release the mutex...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Exiting...");
        }
        finally
        {
            mutex.ReleaseMutex();
            mutex.Dispose();
        }      
    }
}
