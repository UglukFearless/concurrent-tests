
namespace ConcurrentTests.Examples;

internal class _08_DeadlockDemo : IExample
{
    public string Name => "Deadlock Demo";
    public string ShortDescription => "Creates a deadlock by acquiring locks in the wrong order.";

    private readonly object _lock1 = new();
    private readonly object _lock2 = new();

    public async Task DoAction()
    {
        Console.WriteLine("Deadlock demo: two threads, two locks, wrong order");
        Console.WriteLine();

        var task1 = Task.Run(() =>
        {
            lock (_lock1)
            {
                Console.WriteLine("Task 1: acquired lock 1");
                Thread.Sleep(100);
                Console.WriteLine("Task 1: trying to acquire lock 2...");
                lock (_lock2)
                {
                    Console.WriteLine("Task 1: acquired lock 2");
                }
            }
        });

        var task2 = Task.Run(() =>
        {
            lock (_lock2)
            {
                Console.WriteLine("Task 2: acquired lock 2");
                Thread.Sleep(100);
                Console.WriteLine("Task 2: trying to acquire lock 1...");
                lock (_lock1)
                {
                    Console.WriteLine("Task 2: acquired lock 1");
                }
            }
        });

        await Task.Delay(2000);

        Console.WriteLine();
        Console.WriteLine("❌ Deadlock occurred!");
        Console.WriteLine("   Task 1 holds lock 1, waiting for lock 2");
        Console.WriteLine("   Task 2 holds lock 2, waiting for lock 1");

        await Task.WhenAll(task1, task2);
    }
}
