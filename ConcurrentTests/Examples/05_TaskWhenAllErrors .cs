
namespace ConcurrentTests.Examples;

internal class _05_TaskWhenAllErrors : IExample
{
    public async Task DoAction()
    {
        var tasks = Enumerable.Range(0, 5).Select(i => Task.Run(() =>
        {
            if (i % 2 == 0)
            {
                throw new Exception($"Exception from {i} task!");
            }

            Console.WriteLine($"Task {i}: success");
        })).ToArray();

        var all = Task.WhenAll(tasks);

        try
        {
            await all;
        }
        catch (Exception) 
        {
            if (all.Exception != null)
            {
                Console.WriteLine($"\nTotal exceptions: {all.Exception.InnerExceptions.Count}");
                foreach (var ex in all.Exception.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
