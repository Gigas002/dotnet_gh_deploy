namespace Deploy.Server.Tests.ConfiguratorsTests;

#pragma warning disable CS1591
#pragma warning disable CS8625

public class WebApplicationBuilderConfiguratorTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void ConfigureTest()
    {
        // swashbuckle
        bool isSwashbuckle = true;
        Assert.DoesNotThrow(() => WebApplicationBuilderConfigurator.Configure(null, isSwashbuckle));

        // nswag
        isSwashbuckle = false;
        Assert.DoesNotThrow(() => WebApplicationBuilderConfigurator.Configure(null, isSwashbuckle));
    }
}
