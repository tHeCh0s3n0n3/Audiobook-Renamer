using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser(false)]
public class Benchy
{
    private readonly string _directory = "D:\\Audiobooks\\Open Audible\\books";

    [Benchmark]
    public void IDSharp()
    {
        _ = IdSharpTest.ParseDirectory(_directory);
    }

    [Benchmark]
    public void TagLibSharp()
    {
        _ = TagLibSharpTest.ParseDirectory(_directory);
    }
}
