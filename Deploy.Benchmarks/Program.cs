using BenchmarkDotNet.Running;

namespace Deploy.Benchmarks;

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<Md5VsSha256>();
    }
}
