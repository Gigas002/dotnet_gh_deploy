using Deploy.Core;
using Microsoft.EntityFrameworkCore;

namespace Deploy.Server;

#pragma warning disable CS1591
#pragma warning disable CA2007

public static class DbContextConfigurator
{
    public const string DataSource = "Data Source=../deploy.db";

    public static async Task ConfigureAsync(DbContextOptions<Context> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        await using var db = new Context(options);

        await db.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

        var vasya = new User { Name = "Vasya", Age = 40 };
        var petya = new User { Name = "Petya", Age = 30 };
        var katya = new User { Name = "Katya", Age = 20 };

        await db.AddRangeAsync(vasya, petya, katya).ConfigureAwait(false);

        await db.SaveChangesAsync().ConfigureAwait(false);
    }
}
