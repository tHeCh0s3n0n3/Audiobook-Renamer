using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using static MetadataHelper.Helper;

namespace Benchmarks;

[MemoryDiagnoser(false)]
public class Benchy
{
    private readonly string _directory = "D:\\Audiobooks\\Open Audible\\books";

    [Benchmark]
    [BenchmarkCategory("MP3")]
    public void IDSharp_MP3()
    {
        _ = IdSharpTest.ParseDirectory(_directory, FileTypes.MP3);
    }

    [Benchmark]
    [BenchmarkCategory("MP3")]
    public void TagLibSharp_MP3()
    {
        _ = TagLibSharpTest.ParseDirectory(_directory, FileTypes.MP3);
    }

    [Benchmark]
    [BenchmarkCategory("MP3")]
    public void ATL_MP3()
    {
        _ = ATLTests.ParseDirectory(_directory, FileTypes.MP3);
    }

    [Benchmark]
    [BenchmarkCategory("M4B")]
    public void IDSharp_M4B()
    {
        _ = IdSharpTest.ParseDirectory(_directory, FileTypes.M4B);
    }

    [Benchmark]
    [BenchmarkCategory("M4B")]
    public void TagLibSharp_M4B()
    {
        _ = TagLibSharpTest.ParseDirectory(_directory, FileTypes.M4B);
    }

    [Benchmark]
    [BenchmarkCategory("M4B")]
    public void ATL_M4B()
    {
        _ = ATLTests.ParseDirectory(_directory, FileTypes.M4B);
    }
}
