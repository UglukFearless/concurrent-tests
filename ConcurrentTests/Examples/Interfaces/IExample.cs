
namespace ConcurrentTests.Examples;

internal interface IExample
{
    Task DoAction();
    string Name { get; }
    string ShortDescription { get; }
}
