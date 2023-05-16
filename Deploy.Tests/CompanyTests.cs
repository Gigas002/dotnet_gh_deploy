namespace Deploy.Tests;

public class CompanyTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        Company company = new Company();

        Assert.IsNotNull(company);
    }

    [Test]
    public void PropertiesTest()
    {
        // ctor and setters
        Company company = new Company(1, "microsoft");

        // id
        var id = company.Id;
        Assert.That(id, Is.EqualTo(company.Id));

        // name
        var name = company.Name;
        Assert.That(name, Is.EqualTo(company.Name));

        // users
        var mockUsers = new Mock<IEnumerable<User>>();
        company.Users = mockUsers.Object;
        Assert.NotNull(company.Users);
    }

    [Test]
    public void ToStringTest()
    {
        var actual = "microsoft";
        Company company = new Company(1, actual);

        Assert.That(actual, Is.EqualTo(company.ToString()));
    }
}
