using Microsoft.EntityFrameworkCore;

namespace Deploy.Server.Tests.ConfiguratorsTests;

#pragma warning disable CS1591
#pragma warning disable CA2007
#pragma warning disable CS8625

public class DbContextConfiguratorTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void ConfigureAsyncTest()
    {
        var contextOptions = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(nameof(DbContextConfiguratorTests))
            .Options;

        Assert.DoesNotThrowAsync(async () => await DbContextConfigurator.ConfigureAsync(contextOptions));
        Assert.ThrowsAsync<ArgumentNullException>(async () => await DbContextConfigurator.ConfigureAsync(null));
    }
}
