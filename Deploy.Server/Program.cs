using Deploy.Core;
using Microsoft.EntityFrameworkCore;

namespace Deploy.Server;

#pragma warning disable CS1591

public static class Program
{
    private static readonly DbContextOptionsBuilder<Context> _contextOptions = new DbContextOptionsBuilder<Context>()
                                                                        .UseSqlite(DbContextConfigurator.DataSource)
                                                                        .UseSnakeCaseNamingConvention();

    public static async Task Main(string[] args)
    {
        const bool isSwashbuckle = true;
        var builder = WebApplicationBuilderConfigurator.Configure(args, isSwashbuckle);
        var application = WebApplicationConfigurator.Configure(builder, isSwashbuckle);

        await DbContextConfigurator.ConfigureAsync(_contextOptions.Options).ConfigureAwait(false);

        await application.RunAsync().ConfigureAwait(false);
    }
}
