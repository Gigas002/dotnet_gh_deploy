using Microsoft.EntityFrameworkCore;
using Deploy.Core;

namespace Deploy.Cli;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning disable CS1591

public static class Program
{
    private static DbContextOptionsBuilder<Context> _contextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlite("Data Source=../deploy.db").UseSnakeCaseNamingConvention();

    public static async Task Main(string[] args)
    {
        bool runTest = args?.Length > 0;

        if (runTest)
            await TestDb().ConfigureAwait(false);
        else
            Console.WriteLine("Hello world!");
    }

    public static async Task TestDb()
    {
        // add data
        using (var db = new Context(_contextOptions.Options))
        {
            var user1 = new User { Name = "Bob", Age = 30 };
            var user2 = new User { Name = "Tom", Age = 21 };
            var user3 = new User { Name = "Alice", Age = 25 };

            var company1 = new Company { Name = "Microsoft" };
            var company2 = new Company { Name = "Apple" };

            user1.Company = company1;
            user2.Company = company2;

            await db.Users.AddRangeAsync(user1, user2, user3).ConfigureAwait(false);
            await db.Companies.AddRangeAsync(company1, company2).ConfigureAwait(false);

            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        // get data
        using (var db = new Context(_contextOptions.Options))
        {
            foreach (var user in db.Users.Include(u => u.Company))
            {
                Console.WriteLine($"Name: {user.Name}, Age: {user.Age}, Company: {user.Company}");
            }
        }
    }
}
