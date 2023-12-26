namespace Deploy.Tests;

#pragma warning disable CS8625
#pragma warning disable CS1591

public class ContextTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void ConstructorTest()
    {
        var options = new DbContextOptions<Context>();
        var db = new Context(options);
        
        Assert.That(db, Is.Not.Null);
        Assert.Throws<ArgumentNullException>(() => 
        {
            _ = new Context(null);
        });

        db.Dispose();
    }

    [Test]
    public void PropertiesTest()
    {
        // ctor and setters
        var options = new DbContextOptions<Context>();
        var db = new Context(options);

        // users
        var mockUsers = new Mock<DbSet<User>>();
        db.Users = mockUsers.Object;
        Assert.That(db.Users, Is.Not.Null);

        // companies
        var mockCompanies = new Mock<DbSet<Company>>();
        db.Companies = mockCompanies.Object;
        Assert.That(db.Companies, Is.Not.Null);

        db.Dispose();
    }
}
