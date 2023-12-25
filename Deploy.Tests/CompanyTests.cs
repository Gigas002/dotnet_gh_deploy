namespace Deploy.Tests;

#pragma warning disable CS1591

public class CompanyTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        var company = new Company();

        Assert.That(company, Is.Not.Null);
    }

    [Test]
    public void PropertiesTest()
    {
        // ctor and setters
        var company = new Company(1, "microsoft");

        // id
        var id = company.Id;
        Assert.That(id, Is.EqualTo(company.Id));

        // name
        var name = company.Name;
        Assert.That(name, Is.EqualTo(company.Name));

        // users
        var mockUsers = new Mock<IEnumerable<User>>();
        company.Users = mockUsers.Object;
        Assert.That(company.Users, Is.Not.Null);
    }

    [Test]
    public void ToStringTest()
    {
        var actual = "microsoft";
        Company company = new Company(1, actual);

        Assert.That(actual, Is.EqualTo(company.ToString()));
    }
}
