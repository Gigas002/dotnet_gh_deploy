using BenchmarkDotNet.Running;

namespace Deploy.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Md5VsSha256>();
    }
}
