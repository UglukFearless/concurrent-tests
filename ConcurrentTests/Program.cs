using ConcurrentTests.Examples;

Console.WriteLine("Concurrent programming examples started.");
Console.WriteLine();

List<IExample> examples = new List<IExample>
{
    new _01_LockCounter(),
    new _02_SemaphoreDownloader(),
    new _03_MutexSingleInstance(),
    new _04_AutoResetSignaling(),
    new _05_TaskWhenAllErrors(),
    new _06_InterlockedCounter(),
    new _07_CancellationTokenDemo(),
    new _08_DeadlockDemo(),
    new _09_ValueTaskCache(),
};

do
{
    OutputExamplesList(examples);

    var input = 0;
    try
    {
        input = ReadAndValidateInput(examples);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        continue;
    }

    if (input == 0)
        break;

    var example = examples[input - 1];
    await ExecuteExample(example);
    continue;

} while (true);

Console.WriteLine("Program exit...");


void OutputExamplesList(List<IExample> examples)
{
    Console.WriteLine("Available examples:");
    for (var i = 0; i < examples.Count; i++)
    {
        var example = examples[i];
        Console.WriteLine($"{i + 1} {example.Name} - {example.ShortDescription}");
    }
    Console.WriteLine();
}

int ReadAndValidateInput(List<IExample> examples)
{
    Console.WriteLine("Enter an example number to run, or 0 to exit:");
    var input = Console.ReadLine();

    if (input == null || !int.TryParse(input.Trim(), out var value))
    {
        throw new ArgumentException("Invalid input");
    }

    if (value > examples.Count)
    {
        throw new ArgumentOutOfRangeException("Invalid example number - out of range.");
    }

    return value;
}

async Task ExecuteExample(IExample example)
{
    Console.WriteLine($"Example {example.Name} started...");
    await example.DoAction();
    Console.WriteLine($"Example {example.Name} ended.");
    Console.WriteLine();
}