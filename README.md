# ConcurrentTests

Примеры приёмов работы с многопоточностью и асинхронностью в C#.

## Компиляция

```bash
dotnet build
```

## Запуск

```bash
dotnet run --project ConcurrentTests
```

## Примеры

- `01_LockCounter.cs` — синхронизация доступа к счётчику с помощью `lock`.
- `02_SemaphoreDownloader.cs` — ограничение числа параллельных задач через `SemaphoreSlim`.
- `03_MutexSingleInstance.cs` — запуск только одного экземпляра приложения через именованный `Mutex`.
- `04_AutoResetSignaling.cs` — различие между `AutoResetEvent` и `ManualResetEvent`.
- `05_TaskWhenAllErrors.cs` — сбор всех исключений из группы задач через `Task.WhenAll`.
- `06_InterlockedCounter.cs` — сравнение инкремента через `lock` и `Interlocked`.
- `07_CancellationTokenDemo.cs` — отмена ожидания `SemaphoreSlim` по `CancellationToken`.
- `08_DeadlockDemo.cs` — демонстрация deadlock при неправильном порядке захвата блокировок.
- `09_ValueTaskCache.cs` — сравнение `Task` и `ValueTask` на сценарии чтения из кэша.