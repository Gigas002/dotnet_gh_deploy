using Microsoft.EntityFrameworkCore;
using Deploy.Core;

namespace Deploy.Cli;

public class Program
{
    public static async Task Main()
    {
        // add data
        await using (var db = new Context())
        {
            var user1 = new User() { Name = "Bob", Age = 30 };
            var user2 = new User() { Name = "Tom", Age = 21 };
            var user3 = new User() { Name = "Alice", Age = 25 };

            var company1 = new Company() { Name = "Microsoft" };
            var company2 = new Company() { Name = "Apple" };

            user1.Company = company1;
            user2.Company = company2;

            await db.Users.AddRangeAsync(user1, user2, user3);
            await db.Companies.AddRangeAsync(company1, company2);

            await db.SaveChangesAsync();
        }

        // get data
        await using (var db = new Context())
        {
            foreach (var user in db.Users.Include(u => u.Company))
            {
                Console.WriteLine($"Name: {user.Name}, Age: {user.Age}, Company: {user.Company}");
            }
        }
    }
}
