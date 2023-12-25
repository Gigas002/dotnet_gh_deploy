using BenchmarkDotNet.Running;

namespace Deploy.Benchmarks;

#pragma warning disable CS1591

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<Md5VsSha256>();
    }
}
