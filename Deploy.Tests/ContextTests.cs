using Microsoft.EntityFrameworkCore;

namespace Deploy.Tests;

public class ContextTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        var db = new Context();

        Assert.That(db, Is.Not.Null);

        db.Dispose();
    }

    [Test]
    public void PropertiesTest()
    {
        // ctor and setters
        var db = new Context();

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

    [Test]
    public void OnConfiguringTest()
    {
        // ctor and setters
        var db = new Context();

        // users
        var mockUsers = new Mock<DbSet<User>>();
        db.Users = mockUsers.Object;
        Assert.That(db.Users, Is.Not.Null);

        // companies
        var mockCompanies = new Mock<DbSet<Company>>();
        db.Companies = mockCompanies.Object;
        Assert.That(db.Companies, Is.Not.Null);

        Assert.DoesNotThrow(() => db.SaveChanges());

        db.Dispose();
    }
}
