namespace Deploy.Server.Tests;

internal sealed class TestCase
{
    public object? Inputs { get; set; }

    public object? Expected { get; set; }

    public bool Throws { get; set; }

    public int ResponseCode { get; set; }
}
