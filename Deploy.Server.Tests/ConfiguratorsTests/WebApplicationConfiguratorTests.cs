namespace Deploy.Server.Tests.ConfiguratorsTests;

#pragma warning disable CS1591
#pragma warning disable CS8625

public class WebApplicationConfiguratorTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void ConfigureTest()
    {
        // swashbuckle
        bool isSwashbuckle = true;
        var builder = WebApplicationBuilderConfigurator.Configure(null, isSwashbuckle);
        Assert.DoesNotThrow(() => WebApplicationConfigurator.Configure(builder, isSwashbuckle));
        Assert.Throws<ArgumentNullException>(() => WebApplicationConfigurator.Configure(null, isSwashbuckle));

        // nswag
        isSwashbuckle = false;
        builder = WebApplicationBuilderConfigurator.Configure(null, isSwashbuckle);
        Assert.DoesNotThrow(() => WebApplicationConfigurator.Configure(builder, isSwashbuckle));
        Assert.Throws<ArgumentNullException>(() => WebApplicationConfigurator.Configure(null, isSwashbuckle));
    }
}
