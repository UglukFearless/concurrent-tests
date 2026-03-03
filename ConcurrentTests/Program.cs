using ConcurrentTests.Examples;

Console.WriteLine("Concurrent programming examples:");
Console.WriteLine();

_01_LockCounter example01 = new();
await example01.DoAction();
Console.WriteLine();

_02_SemaphoreDownloader example02 = new();
await example02.DoAction();
Console.WriteLine();

_03_MutexSingleInstance example03 = new();
await example03.DoAction();
Console.WriteLine();

Console.WriteLine("Done.");
