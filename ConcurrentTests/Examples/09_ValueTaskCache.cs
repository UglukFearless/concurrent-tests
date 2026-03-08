

using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;
using System.Diagnostics;

namespace ConcurrentTests.Examples;

internal class _09_ValueTaskCache : IExample
{
    public string Name => "ValueTask Cache";
    public string ShortDescription => "Compares Task and ValueTask performance in a cache scenario.";

    private const int AmountOfRead = 10000000;
    private readonly MemoryCache _valueTaskCache = new(new MemoryCacheOptions());
    private readonly MemoryCache _taskCache = new(new MemoryCacheOptions());
    private readonly ImmutableDictionary<string, string> _storage = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" },
            { "key4", "value4" },
            { "key5", "value5" },
            { "key6", "value6" },
            { "key7", "value7" },
            { "key8", "value8" },
            { "key9", "value9" },
            { "key10", "value10" },
        }.ToImmutableDictionary();

    public async Task DoAction()
    {
        _taskCache.Clear();
        await DoWithTasks();

        _valueTaskCache.Clear();
        await DoWithValueTasks();
    }

    private async Task DoWithValueTasks()
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        var tasks = Enumerable.Range(0, 10).Select(i => Task.Run(async () =>
        {
            for (var i = 0; i < AmountOfRead; i++)
            {
                var key = $"key{(i % 10 + 1)}";
                await GetWithValueTask(key);
            }
        }));
        await Task.WhenAll(tasks);
        sw.Stop();
        Console.WriteLine($"Time with ValueTasks: {sw.ElapsedMilliseconds} ms");
    }

    private async ValueTask<string> GetWithValueTask(string key)
    {
        if (_valueTaskCache.TryGetValue(key, out var cacheEntry))
        {
            var data = cacheEntry as string;
            if (data != null)
                return data;
        }
        var result = await GetFromStorage(key);
        _valueTaskCache.Set(key, result);
        return result;
    }

    private async Task DoWithTasks()
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        var tasks = Enumerable.Range(0, 10).Select(i => Task.Run(async () =>
        {
            for (var i = 0; i < AmountOfRead; i++)
            {
                var key = $"key{(i % 10 + 1)}";
                await GetWithTask(key);
            }
        }));
        await Task.WhenAll(tasks);
        sw.Stop();
        Console.WriteLine($"Time with Tasks: {sw.ElapsedMilliseconds} ms");
    }

    private async Task<string> GetWithTask(string key)
    {
        if (_taskCache.TryGetValue(key, out var cacheEntry))
        {
            var data = cacheEntry as string;
            if (data != null)
                return data;
        }

        var result = await GetFromStorage(key);
        _taskCache.Set(key, result);
        return result;
    }

    private async Task<string> GetFromStorage(string key)
    {
        if (_storage.ContainsKey(key))
        {
            await Task.Delay(1);
            return _storage[key];
        }
        
        throw new Exception($"Unknown key {key}.");
    }
}
